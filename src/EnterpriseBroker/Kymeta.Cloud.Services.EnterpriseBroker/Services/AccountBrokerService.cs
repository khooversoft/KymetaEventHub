using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Services;

/// <summary>
/// Service used to translate data between salesforce/oracle and OSS
/// </summary>
public interface IAccountBrokerService
{
    /// <summary>
    /// Process account action
    /// </summary>
    /// <param name="model">Incoming payload</param>
    /// <returns>Processed action</returns>
    Task<UnifiedResponse> ProcessAccountAction(SalesforceAccountModel model);
    Task<SalesforceAccountObjectModel> GetSalesforceAccountById(string accountId);
    Task<List<SalesforceAccountObjectModel>> GetSalesforceAccounts();
    Task<string?> UpdateSalesforceAccount(SalesforceAccountObjectModel model);
    Task<int> SyncSalesforceAccountsToOss();
}

public class AccountBrokerService : IAccountBrokerService
{
    private readonly IActionsRepository _actionsRepository;
    private readonly IOracleService _oracleService;
    private readonly IOssService _ossService;
    private readonly ISalesforceClient _sfClient;

    public AccountBrokerService(IActionsRepository actionsRepository, IOracleService oracleService, IOssService ossService, ISalesforceClient salesforceClient)
    {
        _actionsRepository = actionsRepository;
        _oracleService = oracleService;
        _ossService = ossService;
        _sfClient = salesforceClient;
    }

    public async Task<SalesforceAccountObjectModel> GetSalesforceAccountById(string accountId)
    {
        return await _sfClient.GetAccountFromSalesforce(accountId);
    }

    public async Task<List<SalesforceAccountObjectModel>> GetSalesforceAccounts()
    {
        return await _sfClient.GetAccountsFromSalesforce();
    }

    public async Task<UnifiedResponse> ProcessAccountAction(SalesforceAccountModel model)
    {
        /*
         * DETERMINE WHERE TO SYNC
         */
        var syncToOss = model.SyncToOss.GetValueOrDefault();
        var syncToOracle = model.SyncToOracle.GetValueOrDefault();

        /*
         * MARSHAL UP RESPONSE
         */
        #region Build initial response object
        var response = new UnifiedResponse
        {
            SalesforceObjectId = model.ObjectId,
            OracleStatus = syncToOracle ? StatusType.Started : StatusType.Skipped,
            OSSStatus = syncToOss ? StatusType.Started : StatusType.Skipped
        };
        #endregion

        /*
         * LOG THE ENTERPRISE APPLICATION BROKER ACTION
         */
        #region Log the Enterprise Action
        // Serialize the body coming in
        string body = JsonSerializer.Serialize(model, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
        // Create the action record object
        var salesforceTransaction = new SalesforceActionTransaction
        {
            Id = Guid.NewGuid(),
            Object = ActionObjectType.Account,
            ObjectId = model.ObjectId,
            ObjectName = model.Name,
            CreatedOn = DateTime.UtcNow,
            UserName = model.UserName,
            SerializedObjectValues = body,
            LastUpdatedOn = DateTime.UtcNow,
            TransactionLog = new List<SalesforceActionRecord>()
        };

        // Insert the event into the database, receive the response object and update the existing variable
        salesforceTransaction = await _actionsRepository.InsertActionRecord(salesforceTransaction);
        #endregion

        /*
         * 1. Find Oracle Organization by SF Account Id
         * 2. If Organization does not exist, create it in Oracle
         * 3. If Organization exists, update the organization in Oracle
         * 4. Find Customer Account by SF Account Id
         * 5. If Customer Account does not exist, create it in Oracle
         * 6. If Customer Account exists, update the customer account in Oracle
         * 7. Find Customer Account Profile by SF Account Id
         * 8. If Customer Account Profile does not exist, create it in Oracle
         * 9. If Customer Account Profile exists, update the customer account profile in Oracle
         * 10. If model.Addresses is not null and has more than 0 entities, this request is an account Create request, process each address via the AddressService
         * 11. If model.Contacts is not null and has more than 0 entities, this request is an account Create request, process each contact via the ContactService
         * 12. Gather up all the Ids from any created or updated entities, update the statuses, and send back the response to SF
         */

        /*
         * INITIALIZE THE IDS
         */
        string? ossAccountId = null;
        string? oracleOrganizationId = null;
        string? oracleCustomerAccountId = null;
        string? oracleCustomerAccountProfileId = null;
        List<AccountChildResponse> addressesForSalesforce = new List<AccountChildResponse>();
        List<AccountChildResponse> contactsForSalesforce = new List<AccountChildResponse>();

        try
        {
            #region Process Account Create
            /*
             * SEND TO OSS IF REQUIRED
             */
            #region Send to OSS
            if (syncToOss)
            {
                try
                {
                    // fetch the entire account model from salesforce
                    var accountFromSalesforce = await _sfClient.GetAccountFromSalesforce(model.ObjectId);

                    // First, fetch to see if the account exists in OSS. If it does, we do an update. Otherwise we add.
                    var existingAccount = await _ossService.GetAccountBySalesforceId(model.ObjectId);
                    // Set initial OSSStatus response value to Successful. It will get overwritten if there is an error.
                    response.OSSStatus = StatusType.Successful;

                    // If the account exists, it's an update
                    if (existingAccount != null)
                    {
                        // If the account exists, we can set the Id early
                        response.OssAccountId = existingAccount.Id?.ToString();
                        ossAccountId = existingAccount.Id.ToString();
                        // Update the account
                        var updatedAccountTuple = await _ossService.UpdateAccount(model, accountFromSalesforce, salesforceTransaction);
                        // Item1 is the account object -- if it's null, we have a problem
                        if (updatedAccountTuple.Item1 == null)
                        {
                            response.OSSErrorMessage = updatedAccountTuple.Item2;
                            response.OSSStatus = StatusType.Error;
                        }

                        // if children accounts are present, make sure they reference the correct parent Account in OSS
                        if (model.ChildAccounts != null && model.ChildAccounts.Any())
                        {
                            var updateChildrenResult = await _ossService.UpdateChildAccounts(existingAccount, model, salesforceTransaction);
                            if (!updateChildrenResult.Item1)
                            {
                                // update the response to indicate an error took place
                                response.OSSErrorMessage = updateChildrenResult.Item3;
                                response.OSSStatus = StatusType.Error;
                            }
                            // append the updated children to the response
                            response.ChildAccounts = updateChildrenResult.Item2;
                        }
                    }
                    else
                    {
                        // Keep in mind, when adding, we do not fill in the Oracle Id here -- we update it after all the Oracle creation is finished
                        var addedAccountTuple = await _ossService.AddAccount(model, accountFromSalesforce, salesforceTransaction);
                        if (string.IsNullOrEmpty(addedAccountTuple.Item2)) // No error!
                        {
                            response.OssAccountId = addedAccountTuple.Item1.Id.ToString();
                            ossAccountId = addedAccountTuple.Item1?.Id?.ToString();

                            // if children accounts are present, make sure they reference the correct parent in OSS
                            if (model.ChildAccounts != null && model.ChildAccounts.Any())
                            {
                                var updateChildrenResult = await _ossService.UpdateChildAccounts(addedAccountTuple.Item1, model, salesforceTransaction);
                                if (!updateChildrenResult.Item1)
                                {
                                    // update the response to indicate an error took place
                                    response.OSSErrorMessage = updateChildrenResult.Item3;
                                    response.OSSStatus = StatusType.Error;
                                }
                                // append the updated children to the response
                                response.ChildAccounts = updateChildrenResult.Item2;
                            }
                        }
                        else // Is error, do not EXIT..
                        {
                            response.OSSStatus = StatusType.Error;
                            response.OSSErrorMessage = addedAccountTuple.Item2;
                        }
                    }

                    // set this value on the model so Oracle can intake it
                    model.OssId = ossAccountId;
                }
                catch (Exception ex)
                {
                    response.OSSStatus = StatusType.Error;
                    response.OSSErrorMessage = $"Error syncing to OSS due to an exception: {ex.Message}";

                    if (salesforceTransaction.TransactionLog == null) salesforceTransaction.TransactionLog = new List<SalesforceActionRecord>();
                    // Add the salesforce transaction record
                    var actionRecord = new SalesforceActionRecord
                    {
                        ObjectType = ActionObjectType.Account,
                        Action = salesforceTransaction.TransactionLog.OrderByDescending(s => s.Timestamp).FirstOrDefault()?.Action ?? SalesforceTransactionAction.Default,
                        Status = StatusType.Error,
                        Timestamp = DateTime.UtcNow,
                        ErrorMessage = $"Error syncing to OSS due to an exception: {ex.Message}",
                        EntityId = model.ObjectId
                    };
                    salesforceTransaction.TransactionLog?.Add(actionRecord);
                    await _actionsRepository.AddTransactionRecord(salesforceTransaction.Id, salesforceTransaction.Object.ToString() ?? "Unknown", actionRecord);
                }
            }
            #endregion

            /*
             * SEND TO ORACLE IF REQUIRED
             */
            #region Send to Oracle
            if (syncToOracle)
            {
                // There is a plethora of possible exceptions in this flow, so we're going to catch any of them and ensure the logs are written
                try
                {
                    if (string.IsNullOrEmpty(model.ObjectId))
                    {
                        // fatal error occurred when sending request to oracle
                        response.OracleStatus = StatusType.Error;
                        response.OracleErrorMessage = $"Salesforce Object ID is required.";
                        return response;
                    }
                    // We have to create 8 entities in Oracle:
                    // Organization, Location(s), PartySite, Person(s), CustomerAccount, CustomerAccountSite, CustomerAccountContact, CustomerAccountProfile

                    // search for existing Organization objects with Salesforce Id or Oracle Ids (when present)
                    var organizationResult = await _oracleService.GetOrganizationById(model.ObjectId, salesforceTransaction, model.OraclePartyId);
                    if (!organizationResult.Item1)
                    {
                        // fatal error occurred when sending request to oracle
                        response.OracleStatus = StatusType.Error;
                        response.OracleErrorMessage = organizationResult.Item3;
                        return response;
                    }

                    // remap Salesforce Business Unit value to Oracle Address Set
                    var addressSetId = await _oracleService.RemapBusinessUnitToOracleSiteAddressSet(model.BusinessUnit, salesforceTransaction);
                    if (addressSetId == null)
                    {
                        // fatal error occurred when sending request to oracle... return badRequest here?
                        response.OracleStatus = StatusType.Error;
                        response.OracleErrorMessage = $"Business Unit not recognized.";
                        return response;
                    }

                    OracleOrganization? organization = null;
                    var partySitesToCreate = new List<OraclePartySite>();
                    var accountSites = new List<OracleCustomerAccountSite>();
                    var accountContacts = new List<OracleCustomerAccountContact>();

                    if (organizationResult.Item2 != null) organization = organizationResult.Item2;

                    #region Location & PartySite
                    // check to see if Locations exist
                    if (model.Addresses != null && model.Addresses.Count > 0)
                    {
                        // extract list of relevant Id values to use in SOAP requests
                        // extracting ObjectId (SalesforceId), OracleLocationId, and OraclePartySiteId
                        List<Tuple<string, ulong?, ulong?>> addressIds = new();
                        addressIds.AddRange(model.Addresses.Select(a => new Tuple<string, ulong?, ulong?>(a.ObjectId, a.OracleLocationId, a.OraclePartyId)));

                        // find locations by Salesforce Id or LocationId (when present)
                        var locationsResult = await _oracleService.GetLocationsById(addressIds, salesforceTransaction);
                        if (!locationsResult.Item1)
                        {
                            // fatal error occurred when sending request to oracle
                            response.OracleStatus = StatusType.Error;
                            response.OracleErrorMessage = locationsResult.Item3;
                            return response;
                        }

                        // init a List of tasks to create multiple Locations in parallel
                        List<Task<Tuple<OracleLocationModel, string>>> createLocationTasks = new();
                        // create a Location for each address
                        foreach (var address in model.Addresses)
                        {
                            // check the found Locations with the address to see if it has been created already
                            var existingLocation = locationsResult.Item2?.FirstOrDefault(l => l.OrigSystemReference == address.ObjectId || l.LocationId == address.OracleLocationId);
                            if (existingLocation == null)
                            {
                                // create Location & OrgPartySite as a list of tasks to run async (outside of the loop)
                                createLocationTasks.Add(_oracleService.CreateLocation(address, salesforceTransaction));
                            }
                            else
                            {
                                // check the Organization PartySites with the address to see if the PartySite has been created
                                // if not, add it to the list to create along with any other new Locations
                                var orgPartySite = organization?.PartySites?.FirstOrDefault(s => s.OrigSystemReference == address.ObjectId || s.LocationId == address.OracleLocationId);
                                if (orgPartySite == null)
                                {
                                    // re-map Salesforce values to Oracle models
                                    var siteUseTypes = Helpers.RemapAddressTypeToOracleSiteUse(address);
                                    // add to the list of Party Sites to create for the current address since it does not have an existing PartySite
                                    partySitesToCreate.Add(new OraclePartySite
                                    {
                                        LocationId = existingLocation.LocationId,
                                        PartySiteName = HttpUtility.HtmlEncode(address.SiteName),
                                        OrigSystemReference = existingLocation.OrigSystemReference,
                                        SiteUses = siteUseTypes
                                    });
                                }
                                else
                                {
                                    // append to the list of accountSites so we can verify the Customer Account has the necessary objects for the Location(s)
                                    accountSites.Add(new OracleCustomerAccountSite
                                    {
                                        OrigSystemReference = orgPartySite.OrigSystemReference,
                                        PartySiteId = orgPartySite.PartySiteId,
                                        SetId = addressSetId,
                                        SiteUses = orgPartySite.SiteUses?.Select(su => new OracleCustomerAccountSiteUse
                                        {
                                            SiteUseCode = su.SiteUseType
                                        }).ToList()
                                    });
                                }
                            }
                        }

                        // check to see if we are creating any Locations
                        if (createLocationTasks.Count > 0)
                        {
                            // execute requests to create Locations in async fashion
                            await Task.WhenAll(createLocationTasks);
                            // get the response data
                            var createLocationResults = createLocationTasks.Select(t => t.Result).ToList();
                            // iterate through the results for the create Location requests
                            for (int i = 0; i < createLocationResults.Count; i++)
                            {
                                var result = createLocationResults[i];
                                // acquire the matching Location so we can set the SiteUseType below
                                var address = model.Addresses.FirstOrDefault(a => a.ObjectId == result.Item1.OrigSystemReference);

                                if (result.Item1 == null)
                                {
                                    // TODO: what action should we take here? Alert of some sort?
                                    // create location failed for some reason
                                    Console.WriteLine($"[DEBUG] Error: {result.Item2}");
                                }
                                else
                                {
                                    // re-map Salesforce values to Oracle models
                                    var siteUseTypes = Helpers.RemapAddressTypeToOracleSiteUse(address);

                                    // Location was created successfully... so add to the list so we can create a Party Site record for it
                                    partySitesToCreate.Add(new OraclePartySite
                                    {
                                        LocationId = result.Item1.LocationId,
                                        PartySiteName = HttpUtility.HtmlEncode(address.SiteName),
                                        OrigSystemReference = result.Item1.OrigSystemReference,
                                        SiteUses = siteUseTypes
                                    });
                                }
                            }
                        }
                    }
                    #endregion

                    #region Organization
                    // If Organization does not exist, create it
                    if (organization == null)
                    {
                        // create the Organization in Oracle (include PartySites here because they can be created in the same request with the Organization)
                        var addedOrganization = await _oracleService.CreateOrganization(model, partySitesToCreate, salesforceTransaction);
                        if (addedOrganization.Item1 == null)
                        {
                            // fatal error occurred
                            response.OracleStatus = StatusType.Error;
                            response.OracleErrorMessage = addedOrganization.Item2;
                            return response;
                        }
                        organization = addedOrganization.Item1;
                        oracleOrganizationId = addedOrganization.Item1.PartyNumber.ToString();
                        response.OracleOrganizationId = oracleOrganizationId;

                        // call to REST service to Update the Organization... so we can set the TaxIdentificationNumber... yes...it is very dumb but there is now known alternative.
                        var updatedOrganization = await _oracleService.UpdateOrganization(organization, model, salesforceTransaction);
                        if (updatedOrganization.Item1 == null)
                        {
                            // fatal error occurred
                            response.OracleStatus = StatusType.Error;
                            response.OracleErrorMessage = updatedOrganization.Item2;
                            return response;
                        }

                        // get response PartySites to append to accountSite list
                        if (organization.PartySites != null && organization.PartySites.Count > 0)
                        {
                            // append to the list of accountSites so we can verify the Customer Account has the necessary objects for the Location(s)
                            accountSites.AddRange(organization.PartySites.Select(ps => new OracleCustomerAccountSite
                            {
                                OrigSystemReference = ps.OrigSystemReference,
                                PartySiteId = ps.PartySiteId,
                                SetId = addressSetId,
                                SiteUses = ps.SiteUses?.Select(su => new OracleCustomerAccountSiteUse
                                {
                                    SiteUseCode = su.SiteUseType
                                }).ToList()
                            }));

                            // Add to response container
                            addressesForSalesforce.AddRange(organization.PartySites.Select(cpr => new AccountChildResponse
                            {
                                OracleId = cpr.PartySiteNumber?.ToString(),
                                SalesforceId = cpr.OrigSystemReference,
                                OracleEntityType = "Location"
                            }).ToList());
                        }
                    }
                    else // Otherwise, update it
                    {
                        // update the Organization
                        var updatedOrganization = await _oracleService.UpdateOrganization(organization, model, salesforceTransaction);
                        if (updatedOrganization.Item1 == null)
                        {
                            // fatal error occurred
                            response.OracleStatus = StatusType.Error;
                            response.OracleErrorMessage = updatedOrganization.Item2;
                            return response;
                        }
                        organization = updatedOrganization.Item1;
                        oracleOrganizationId = organization.PartyNumber.ToString();
                        response.OracleOrganizationId = oracleOrganizationId;

                        // check to see if we need to create any PartySites for the Organization & Locations
                        if (partySitesToCreate.Count > 0)
                        {
                            // create Organization PartySite (batched into a single request for all Locations)
                            var createPartySitesResult = await _oracleService.CreateOrganizationPartySites(organization.PartyId, partySitesToCreate, salesforceTransaction);
                            if (createPartySitesResult.Item1 == null)
                            {
                                // create PartySites failed for some reason
                                Console.WriteLine($"[DEBUG] Error: {createPartySitesResult.Item2}");
                                // fatal error occurred
                                response.OracleStatus = StatusType.Error;
                                response.OracleErrorMessage = createPartySitesResult.Item2;
                                return response;
                            }
                            else
                            {
                                // map created PartySites to list of OracleCustomerAccountSites to create below
                                var sites = createPartySitesResult.Item1.Select(cpr => new OracleCustomerAccountSite
                                {
                                    PartySiteId = cpr.PartySiteId,
                                    OrigSystemReference = cpr.OrigSystemReference,
                                    SetId = addressSetId,
                                    SiteUses = cpr.SiteUses?.Select(su => new OracleCustomerAccountSiteUse
                                    {
                                        SiteUseCode = su.SiteUseType
                                    }).ToList()
                                });
                                accountSites.AddRange(sites);

                                // Add to response container
                                addressesForSalesforce.AddRange(createPartySitesResult.Item1.Select(cpr => new AccountChildResponse
                                {
                                    OracleId = cpr.PartySiteNumber?.ToString(),
                                    SalesforceId = cpr.OrigSystemReference,
                                    OracleEntityType = "Location"
                                }).ToList());
                            }
                        }
                    }
                    #endregion

                    #region Person
                    // verify we have contacts
                    if (model.Contacts != null && model.Contacts.Count > 0)
                    {
                        // verify we have at least one valid Contact record (Contact with a role set to: Primary, Bill To, or Ship To)
                        var contactsWithProperRoles = model.Contacts.Where(c => {
                            var role = c.Role?.ToLower();
                            if (role == null) return false;
                            return role.Contains("primary") || role.Contains("bill") || role.Contains("ship");
                        });
                        if (contactsWithProperRoles.Count() == 0)
                        {
                            // no Contacts found that meet acceptable criteria
                            response.OracleStatus = StatusType.Error;
                            response.OracleErrorMessage = "You must have at least one Contact with a role set to: Primary, Bill To, or Ship To.";
                            return response;
                        }

                        // extract list of relevant Id values to use in SOAP requests
                        // extracting ObjectId (SalesforceId), OraclePartyId
                        List<Tuple<string, ulong?>> contactIds = new();
                        contactIds.AddRange(model.Contacts.Select(c => new Tuple<string, ulong?>(c.ObjectId, c.OraclePartyId)));

                        // find Persons by Salesforce or Oracle Id
                        var personsResult = await _oracleService.GetPersonsById(contactIds, salesforceTransaction);
                        if (!personsResult.Item1)
                        {
                            // fatal error occurred when sending request to oracle
                            response.OracleStatus = StatusType.Error;
                            response.OracleErrorMessage = personsResult.Item3;
                            return response;
                        }

                        // create a Person for each contact
                        foreach (var contact in model.Contacts)
                        {
                            var responsibilityTypes = OracleSoapTemplates.GetResponsibilityType(contact.Role);
                            if (responsibilityTypes == null || responsibilityTypes.Count == 0)
                            {
                                // if the Contact Role was not recognized, ignore the Contact
                                continue;
                            }
                            // check the found Persons with the contact to see if they have been created already
                            var existingContact = personsResult.Item2?.FirstOrDefault(l => l.OrigSystemReference == contact.ObjectId || l.PartyId == contact.OraclePartyId);
                            if (existingContact == null)
                            {
                                // create Person requests as a list of tasks to run async (outside of the loop)
                                // unable to use Task.WhenAll because Oracle is complaining... response from Oracle:
                                // JBO-26092: Failed to lock the record in table HZ_ORGANIZATION_PROFILES with key oracle.jbo.Key[300000100251313], another user holds the lock.
                                var addedPersonResult = await _oracleService.CreatePerson(contact, organization.PartyId, salesforceTransaction);
                                if (addedPersonResult.Item1 == null)
                                {
                                    // TODO: what action should we take here? Alert of some sort?
                                    Console.WriteLine($"[DEBUG] Error: {addedPersonResult.Item2}");
                                }
                                else
                                {
                                    // Person was created successfully... add it to the list so we can check it against the Customer Account Contacts
                                    accountContacts.Add(new OracleCustomerAccountContact
                                    {
                                        ContactPersonId = addedPersonResult.Item1.PartyId,
                                        OrigSystemReference = contact.ObjectId,
                                        RelationshipId = addedPersonResult.Item1.RelationshipId,
                                        ResponsibilityTypes = responsibilityTypes,
                                        IsPrimary = contact.IsPrimary
                                    });

                                    contactsForSalesforce.Add(new AccountChildResponse
                                    {
                                        OracleId = addedPersonResult.Item1.PartyNumber?.ToString(),
                                        SalesforceId = addedPersonResult.Item1.OrigSystemReference,
                                        OracleEntityType = "Person"
                                    });
                                }
                            }
                            else
                            {
                                // we do not need to update the Contact here because the edit Contact action in the ContactBroker will handle updating a Contact
                                // add to `persons` list so we can check Customer Account to ensure the Customer Account Contact exists (or create it)
                                accountContacts.Add(new OracleCustomerAccountContact
                                {
                                    ContactPersonId = existingContact.PartyId,
                                    OrigSystemReference = existingContact.OrigSystemReference,
                                    RelationshipId = existingContact.RelationshipId,
                                    ResponsibilityTypes = responsibilityTypes,
                                    IsPrimary = contact.IsPrimary
                                });
                            }
                        }
                    }
                    #endregion

                    #region Customer Account
                    // search for existing Customer Account records based on Name and Salesforce Id
                    var existingCustomerAccount = await _oracleService.GetCustomerAccountById(model.ObjectId, salesforceTransaction, model.OraclePartyId);
                    if (!existingCustomerAccount.Item1)
                    {
                        // fatal error occurred when sending request to oracle
                        response.OracleStatus = StatusType.Error;
                        response.OracleErrorMessage = existingCustomerAccount.Item3;
                        return response;
                    }

                    // init our simplified model
                    var customerAccount = new OracleCustomerAccount();
                    // If Customer Account does not exist, create it
                    if (existingCustomerAccount.Item2 == null)
                    {
                        var addedCustomerAccount = await _oracleService.CreateCustomerAccount(organization.PartyId, model, accountSites, accountContacts, salesforceTransaction);
                        if (addedCustomerAccount.Item1 == null)
                        {
                            // error creating the Customer Account.... indicate failure
                            response.OracleStatus = StatusType.Error;
                            response.OracleErrorMessage = addedCustomerAccount.Item2;
                            return response;
                        }
                        customerAccount = addedCustomerAccount.Item1;
                        oracleCustomerAccountId = addedCustomerAccount.Item1.AccountNumber.ToString();
                    }
                    else // Otherwise, update it
                    {
                        var existingAccount = existingCustomerAccount.Item2;

                        // review the list of Persons
                        // check for existing Customer Account Contacts to avoid trying to establish a new relationship with an existing Contact
                        if (existingAccount.Contacts != null && existingAccount.Contacts.Count > 0)
                        {
                            // remove the person from the list if they already exist as a contact on the Customer Account
                            var existingContacts = existingAccount.Contacts.Select(s => s.ContactPersonId).ToList();
                            accountContacts.RemoveAll(p => existingContacts.Contains(p.ContactPersonId));
                        }

                        // review list of PartySites/Locations to see if we need to create Customer Account Site records
                        if (existingAccount.Sites != null && existingAccount.Sites.Count > 0)
                        {
                            // check to see if the this site already has been established as a Customer Account Site
                            // remove the partySite from the list if it already exists on the Customer Account
                            var existingSites = existingAccount.Sites.Select(s => s.PartySiteId).ToList();
                            accountSites.RemoveAll(ps => existingSites.Contains(ps.PartySiteId));
                        }

                        // update the Customer Account
                        var updateCustomerAccountResult = await _oracleService.UpdateCustomerAccount(existingAccount, model, accountSites, accountContacts, salesforceTransaction);
                        if (updateCustomerAccountResult.Item1 == null)
                        {
                            // error updating the Customer Account.... indicate failure
                            response.OracleStatus = StatusType.Error;
                            response.OracleErrorMessage = updateCustomerAccountResult.Item2;
                            return response;
                        }
                        customerAccount = updateCustomerAccountResult.Item1;
                        oracleCustomerAccountId = customerAccount?.AccountNumber?.ToString();
                    }
                    #endregion

                    #region Customer Profile
                    // if no Customer Profile exists, this request will return a 500 result (Internal Server Error)... which is super lame.
                    // we are accounting for it in the WebException that is thrown within OracleService to determine if the record does not exist
                    // so we are handling it (somewhat) gracefully
                    var existingCustomerAccountProfile = await _oracleService.GetCustomerProfileByAccountNumber(customerAccount.AccountNumber?.ToString(), salesforceTransaction);
                    if (!existingCustomerAccountProfile.Item1)
                    {
                        // TODO: fatal error occurred when sending request to oracle... return badRequest here?
                        response.OracleStatus = StatusType.Error;
                        response.OracleErrorMessage = existingCustomerAccountProfile.Item3;
                        return response;
                    }
                    // If Customer Account does not exist, create it
                    if (existingCustomerAccountProfile.Item2 == null)
                    {
                        var addedCustomerAccountProfile = await _oracleService.CreateCustomerAccountProfile(customerAccount.PartyId, (uint)customerAccount.AccountNumber, salesforceTransaction);
                        oracleCustomerAccountProfileId = addedCustomerAccountProfile.Item1?.PartyId?.ToString();
                    }
                    else
                    {
                        oracleCustomerAccountProfileId = existingCustomerAccountProfile?.Item2?.PartyId?.ToString();
                    }
                    #endregion

                    response.OracleStatus = StatusType.Successful;
                    response.OracleCustomerAccountId = oracleCustomerAccountId;
                    response.OracleOrganizationId = oracleOrganizationId;
                    response.OracleCustomerProfileId = oracleCustomerAccountProfileId;
                    if (model.Addresses != null && model.Addresses.Count > 0) response.Contacts = contactsForSalesforce;
                    if (model.Contacts != null && model.Contacts.Count > 0) response.Addresses = addressesForSalesforce;

                    // Update OSS with Oracle Id if need be
                    if (syncToOss && !string.IsNullOrEmpty(oracleCustomerAccountId)) // TODO: Is the customer account Id actually what we want?
                    {
                        await _ossService.UpdateAccountOracleId(model, oracleCustomerAccountId, salesforceTransaction);
                    }
                }
                catch (Exception ex)
                {
                    response.OracleStatus = StatusType.Error;
                    response.OracleErrorMessage = $"Error syncing to Oracle due to an exception: {ex.Message}";

                    if (salesforceTransaction.TransactionLog == null) salesforceTransaction.TransactionLog = new List<SalesforceActionRecord>();
                    // Add the salesforce transaction record
                    var actionRecord = new SalesforceActionRecord
                    {
                        ObjectType = ActionObjectType.Account,
                        Action = salesforceTransaction.TransactionLog.OrderByDescending(s => s.Timestamp).FirstOrDefault()?.Action ?? SalesforceTransactionAction.Default,
                        Status = StatusType.Error,
                        Timestamp = DateTime.UtcNow,
                        ErrorMessage = $"Error syncing to Oracle due to an exception: {ex.Message}",
                        EntityId = model.ObjectId
                    };
                    salesforceTransaction.TransactionLog?.Add(actionRecord);
                    await _actionsRepository.AddTransactionRecord(salesforceTransaction.Id, salesforceTransaction.Object.ToString() ?? "Unknown", actionRecord);
                }
            }
            #endregion
            #endregion
        }
        finally
        {
            response.CompletedOn = DateTime.UtcNow;

            // Attach the response to the action log item
            salesforceTransaction.Response = response;
            await _actionsRepository.UpdateActionRecord(salesforceTransaction);
        }

        return response;
    }

    public async Task<string?> UpdateSalesforceAccount(SalesforceAccountObjectModel model)
    {
        try
        {
            await _sfClient.UpdateAccountInSalesforce(model);
            return null;
        } catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public async Task<int> SyncSalesforceAccountsToOss()
    {
        var contacts = await _sfClient.GetContactsFromSalesforce();
        var accounts = await _sfClient.GetAccountsFromSalesforce();
        var justTheIds = accounts.Select(a => a.Id).ToList();
        var ossAccounts = await _ossService.GetAccountsByManySalesforceIds(justTheIds);

        Console.WriteLine($"{contacts.Count} contacts found in Salesforce.");
        Console.WriteLine($"{accounts.Count} accounts found in Salesforce.");

        var remapped = accounts.Select(async (a) => await _ossService.RemapSalesforceAccountToOssAccount(null, a, a.Oracle_Acct__c, contacts, ossAccounts));

        var result = (await Task.WhenAll(remapped)).ToList();

        Console.WriteLine($"Writing {result.Count} to OSS.");

        var response = await _ossService.SyncAccountsToOSS(result);

        return remapped.Count();
    }
}