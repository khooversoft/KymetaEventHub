namespace Kymeta.Cloud.Services.EnterpriseBroker.Services;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;

public interface IAddressBrokerService
{
    Task<UnifiedResponse> ProcessAddressAction(SalesforceAddressModel model);
}

public class AddressBrokerService : IAddressBrokerService
{
    private readonly IActionsRepository _actionsRepository;
    private readonly IOracleService _oracleService;

    public AddressBrokerService(IActionsRepository actionsRepository, IOracleService oracleService)
    {
        _actionsRepository = actionsRepository;
        _oracleService = oracleService;
    }

    public async Task<UnifiedResponse> ProcessAddressAction(SalesforceAddressModel model)
    {
        /*
        * WHERE TO SYNC
        */
        var syncToOss = model.SyncToOss.GetValueOrDefault();
        var syncToOracle = model.SyncToOracle.GetValueOrDefault();

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
            Object = ActionObjectType.Address,
            ObjectId = model.ObjectId,
            ObjectName = model.SiteName,
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

        try
        {
            #region Send to Oracle
            if (syncToOracle)
            {
                // Get Organization by Salesforce Account Id
                var organizationResult = await _oracleService.GetOrganizationById(model.ParentAccountId, salesforceTransaction, model.ParentOraclePartyId);
                if (!organizationResult.Item1 || organizationResult.Item2 == null)
                {
                    response.OracleStatus = StatusType.Error;
                    response.OracleErrorMessage = $"Error syncing Address to Oracle: Organization object with SF reference Id {model.ParentAccountId} was not found.";
                    return response;
                }
                var organization = organizationResult.Item2;

                // Get customer account by Salesforce Account Id
                var customerAccountResult = await _oracleService.GetCustomerAccountById(model.ParentAccountId, salesforceTransaction, model.ParentOraclePartyId);
                if (!customerAccountResult.Item1 || customerAccountResult.Item2 == null)
                {
                    response.OracleStatus = StatusType.Error;
                    response.OracleErrorMessage = $"Error syncing Address to Oracle: Customer Account object with SF reference Id {model.ParentAccountId} was not found.";
                    return response;
                }
                var customerAccount = customerAccountResult.Item2;

                // remap Salesforce Business Unit value to Oracle Address Set
                var addressSetId = await _oracleService.RemapBusinessUnitToOracleSiteAddressSet(model.ParentAccountBusinessUnit, salesforceTransaction);
                if (addressSetId == null)
                {
                    // fatal error occurred when sending request to oracle... return badRequest here?
                    response.OracleStatus = StatusType.Error;
                    response.OracleErrorMessage = $"Business Unit not recognized.";
                    return response;
                }


                // re-map Salesforce values to Oracle models
                var siteUseTypes = Helpers.RemapAddressTypeToOracleSiteUse(model);
                // for the Organization
                var partySitesToCreate = new List<OraclePartySite>();
                var partySitesToUpdate = new List<OraclePartySite>();
                // for the Customer Account
                var accountSites = new List<OracleCustomerAccountSite>();

                var addressIds = new List<Tuple<string, ulong?, ulong?>> {
                    new Tuple<string, ulong?, ulong?>(model.ObjectId, model.OracleLocationId, model.OraclePartyId)
                };

                // search for existing location
                var locationsResult = await _oracleService.GetLocationsById(addressIds, salesforceTransaction);
                if (!locationsResult.Item1)
                {
                    response.OracleStatus = StatusType.Error;
                    response.OracleErrorMessage = $"Error syncing Address to Oracle: {locationsResult.Item3}";
                    return response;
                }
                if (locationsResult.Item2 == null || locationsResult.Item2.Count() == 0)
                {
                    // create new location
                    var createLocationResult = await _oracleService.CreateLocation(model, salesforceTransaction);
                    if (createLocationResult.Item1 == null)
                    {
                        response.OracleStatus = StatusType.Error;
                        response.OracleErrorMessage = $"Error syncing Address to Oracle: Error creating Location: {createLocationResult.Item2}.";
                        return response;
                    }
                    var createdLocation = createLocationResult.Item1;

                    // Location was created successfully... so add to the list so we can create a Party Site record for it
                    partySitesToCreate.Add(new OraclePartySite
                    {
                        LocationId = createLocationResult.Item1.LocationId,
                        PartySiteName = HttpUtility.HtmlEncode(model.SiteName),
                        OrigSystemReference = createLocationResult.Item1.OrigSystemReference,
                        SiteUses = siteUseTypes
                    });
                }
                else
                {
                    // acquire the Location returned from the search
                    var existingLocation = locationsResult.Item2.First();
                    // update the location
                    var updateLocationResult = await _oracleService.UpdateLocation(model, existingLocation, salesforceTransaction);
                    if (updateLocationResult.Item1 == null)
                    {
                        response.OracleStatus = StatusType.Error;
                        response.OracleErrorMessage = $"Error syncing Address to Oracle: Error updating Location: {updateLocationResult.Item2}.";
                        return response;
                    }
                    var updatedLocation = updateLocationResult.Item1;

                    // validate the that PartySite exists for the Organization (if not, create)
                    var orgPartySite = organization.PartySites?.FirstOrDefault(ps => ps.OrigSystemReference == model.ObjectId || ps.PartySiteId == model.OraclePartyId);
                    if (orgPartySite == null)
                    {
                        partySitesToCreate.Add(new OraclePartySite
                        {
                            LocationId = updatedLocation.LocationId,
                            PartySiteName = HttpUtility.HtmlEncode(model.SiteName),
                            OrigSystemReference = updatedLocation.OrigSystemReference,
                            SiteUses = siteUseTypes
                        });
                    }
                    else
                    {
                        // update PartySite
                        orgPartySite.PartySiteName = HttpUtility.HtmlEncode(model.SiteName);
                        partySitesToUpdate.Add(orgPartySite);

                        // set the response Id
                        response.OracleAddressId = orgPartySite.PartySiteNumber?.ToString();

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

                // check to see if we need to create any PartySites for the Organization & Locations
                if (partySitesToCreate.Count > 0)
                {
                    // create Organization PartySite (batched into a single request for all Locations)
                    var createPartySitesResult = await _oracleService.CreateOrganizationPartySites(organization.PartyId, partySitesToCreate, salesforceTransaction);
                    if (createPartySitesResult.Item1 == null)
                    {
                        // create PartySites failed for some reason
                        response.OracleStatus = StatusType.Error;
                        response.OracleErrorMessage = $"Error syncing Address to Oracle: Failed to create Organization Party Sites: {createPartySitesResult.Item2}.";
                        return response;
                    }
                    else
                    {
                        // set the response Id (we should only have one partySite created as this is a solo operation for one Address)
                        response.OracleAddressId = createPartySitesResult.Item1?.FirstOrDefault()?.PartySiteNumber?.ToString();

                        // map created PartySites to list of OracleCustomerAccountSites to create below
                        var sites = createPartySitesResult.Item1?.Select(cpr => new OracleCustomerAccountSite
                        {
                            PartySiteId = cpr.PartySiteId,
                            OrigSystemReference = cpr.OrigSystemReference,
                            SetId = addressSetId,
                            SiteUses = cpr.SiteUses?.Select(su => new OracleCustomerAccountSiteUse
                            {
                                SiteUseCode = su.SiteUseType
                            }).ToList()
                        });
                        // if we have Sites, add them to the list
                        if (sites != null) accountSites.AddRange(sites);
                    }
                }

                // check to see if we need to update any PartySite records for the Organization & Locations
                if (partySitesToUpdate.Count > 0)
                {
                    // update Organization PartySite (batched into a single request for all Locations)
                    var updatePartySitesResult = await _oracleService.UpdateOrganizationPartySites(organization.PartyId, partySitesToUpdate, salesforceTransaction);
                    if (updatePartySitesResult.Item1 == null)
                    {
                        // create PartySites failed for some reason
                        response.OracleStatus = StatusType.Error;
                        response.OracleErrorMessage = $"Error syncing Address to Oracle: Failed to update Organization Party Sites: {updatePartySitesResult.Item2}.";
                        return response;
                    }
                }

                // validate that the CustomerAccountSite exists for the Customer Account (if not, create)
                var accountSite = customerAccount.Sites?.FirstOrDefault(s => s.OrigSystemReference == model.ObjectId || s.PartySiteId == model.OraclePartyId);
                if (accountSite == null)
                {
                    // merge/update the existing Account to add the Customer Account Site
                    var customerAccountUpdateResult = await _oracleService.UpdateCustomerAccountChildren(customerAccount, salesforceTransaction, accountSites, null);
                    if (customerAccountUpdateResult.Item1 == null)
                    {
                        // failed to update Customer Account
                        response.OracleStatus = StatusType.Error;
                        response.OracleErrorMessage = $"Error syncing Address to Oracle: Error adding CustomerAccountSite: {customerAccountUpdateResult.Item2}.";
                        return response;
                    }
                }

                // indicate successful request
                response.OracleStatus = StatusType.Successful;
            }
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
}

