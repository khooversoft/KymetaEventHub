using Kymeta.Cloud.Logging.Activity;
using Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External;
using Newtonsoft.Json;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Services;

/// <summary>
/// Service used to translate data between salesforce/oracle and OSS
/// </summary>
public interface IOssService
{
    /// <summary>
    /// Adds a new account to the OSS service
    /// </summary>
    /// <param name="account">Account model</param>
    /// <returns>Added account</returns>
    Task<Tuple<AccountV2, string>> AddAccount(SalesforceAccountModel model, SalesforceAccountObjectModel sfModel, SalesforceActionTransaction transaction);
    /// <summary>
    /// Update an existing account to OSS
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<Tuple<AccountV2, string>> UpdateAccount(SalesforceAccountModel model, SalesforceAccountObjectModel sfModel, SalesforceActionTransaction transaction);
    /// <summary>
    /// Update an existing child account to OSS
    /// </summary>
    /// <param name="account"></param>
    /// <param name="transaction"></param>
    /// <param name="userName"></param>
    /// <returns></returns>
    Task<Tuple<AccountV2, string>> UpdateChildAccount(AccountV2 account, string userName, SalesforceActionTransaction transaction);
    /// <summary>
    /// Utility method to update a list of child Accounts in OSS
    /// </summary>
    /// <param name="existingAccount"></param>
    /// <param name="children"></param>
    /// <param name="userName"></param>
    /// <param name="salesforceTransaction"></param>
    /// <returns></returns>
    Task<Tuple<bool, List<AccountV2>?, string>> UpdateChildAccounts(AccountV2 existingAccount, SalesforceAccountModel model, SalesforceActionTransaction salesforceTransaction);
    /// <summary>
    /// Update an existing account's oracle id
    /// </summary>
    /// <param name="salesforceId"></param>
    /// <param name="oracleId"></param>
    /// <param name="transaction"></param>
    /// <returns></returns>
    Task<Tuple<AccountV2, string>> UpdateAccountOracleId(SalesforceAccountModel model, string oracleId, SalesforceActionTransaction transaction);
    /// <summary>
    /// Get an OSS Account by its Salesforce Id
    /// </summary>
    /// <param name="salesforceId"></param>
    /// <returns></returns>
    Task<AccountV2?> GetAccountBySalesforceId(string salesforceId);
    /// <summary>
    /// Get a list of OSS Accounts by many Salesforce Ids
    /// </summary>
    /// <param name="salesforceIds">List of strings which are Salesforce object Ids</param>
    /// <returns></returns>
    Task<List<AccountV2>> GetAccountsByManySalesforceIds(List<string> salesforceIds);
    /// <summary>
    /// Remap Salesforce Account Object to OSS Account Object
    /// </summary>
    /// <param name="model"></param>
    /// <param name="sfModel"></param>
    /// <param name="oracleAccountId"></param>
    /// <param name="allContacts"></param>
    /// <returns></returns>
    Task<AccountV2> RemapSalesforceAccountToOssAccount(
        SalesforceAccountModel model,
        SalesforceAccountObjectModel sfModel,
        string? oracleAccountId = null,
        IEnumerable<SalesforceContactObjectModel>? allContacts = null,
        IEnumerable<AccountV2>? allOssAccounts = null
        );
    /// <summary>
    /// Syncs accounts to OSS from SF
    /// </summary>
    /// <param name="accounts">List of accounts</param>
    /// <returns>String with success or fail</returns>
    Task<string> SyncAccountsToOSS(List<AccountV2> accounts);
}

public class OssService : IOssService
{
    private readonly IConfiguration _config;
    private readonly IAccountsClient _accountsClient;
    private readonly IUsersClient _usersClient;
    private readonly IActionsRepository _actionsRepository;
    private readonly ISalesforceClient _sfClient;
    private readonly IActivityLoggerClient _activityLoggerClient;
    private User _systemUser = new()
    { 
        FirstName = "System", 
        LastName = "User", 
        Email = "kcssystemuser@kymeta.io" 
    };

    public OssService(IConfiguration config, IAccountsClient accountsClient, IUsersClient usersClient, IActionsRepository actionsRepository, IActivityLoggerClient activityLoggerClient, ISalesforceClient sfClient)
    {
        _config = config;
        _accountsClient = accountsClient;
        _usersClient = usersClient;
        _actionsRepository = actionsRepository;
        _sfClient = sfClient;

        // system user configs
        _systemUser.Id = new Guid(config["SystemUserId"]);
        _systemUser.AccountId = new Guid(config["KymetaAccountId"]);
        _activityLoggerClient = activityLoggerClient;
    }

    public async Task<Tuple<AccountV2, string>> AddAccount(SalesforceAccountModel model, SalesforceAccountObjectModel sfModel, SalesforceActionTransaction transaction)
    {
        await LogAction(transaction, SalesforceTransactionAction.CreateAccountInOss, ActionObjectType.Account, StatusType.Started);
        string error = null;
        // Verify exists
        var existingAccount = await GetAccountBySalesforceId(model.ObjectId);
        if (existingAccount != null) // If account exists, return from this action with an error
        {
            error = $"Account with Salesforce ID {model.ObjectId} already exists in the system.";
            await LogAction(transaction, SalesforceTransactionAction.CreateAccountInOss, ActionObjectType.Account, StatusType.Error, null, error);
            return new Tuple<AccountV2, string>(null, error);
        }
        // Get the user from OSS system
        User existingUser = null;
        if (!string.IsNullOrEmpty(model.UserName))
        {
            existingUser = await _usersClient.GetUserByEmail(model.UserName);
        }
        if (existingUser == null) existingUser = _systemUser;

        var billingAddress = model.Addresses?.FirstOrDefault(a => a.Type == "Billing & Shipping"); // This string is a picklist value in SF
        var account = await RemapSalesforceAccountToOssAccount(model, sfModel);
        try
        {
            // add the Account
            var addedAccount = await _accountsClient.AddAccount(account);
            if (!string.IsNullOrEmpty(addedAccount.Item2) || !addedAccount.Item1.Id.HasValue) 
            {
                error = $"There was an error adding the Account to OSS: {addedAccount.Item2}";
                await LogAction(transaction, SalesforceTransactionAction.CreateAccountInOss, ActionObjectType.Account, StatusType.Error, null, error);
                return new Tuple<AccountV2, string>(null, error);
            }

            await LogAction(transaction, SalesforceTransactionAction.CreateAccountInOss, ActionObjectType.Account, StatusType.Successful, addedAccount.Item1.Id.ToString());
            await _activityLoggerClient.AddActivity(ActivityEntityTypes.Accounts, existingUser.Id, existingUser.FullName, addedAccount.Item1.Id, $"{addedAccount.Item1.Name}", "addaccount", null, null, JsonConvert.SerializeObject(addedAccount));

            return new Tuple<AccountV2, string>(addedAccount.Item1, string.Empty);
        } catch (Exception ex)
        {
            error = $"There was an error calling the OSS Accounts service: {ex.Message}";
            await LogAction(transaction, SalesforceTransactionAction.CreateAccountInOss, ActionObjectType.Account, StatusType.Error, null, error);
            return new Tuple<AccountV2, string>(null, error);
        }
    }

    public async Task<Tuple<AccountV2, string>> UpdateAccount(SalesforceAccountModel model, SalesforceAccountObjectModel sfModel, SalesforceActionTransaction transaction)
    {
        await LogAction(transaction, SalesforceTransactionAction.UpdateAccountInOss, ActionObjectType.Account, StatusType.Started);
        string error = null;

        var existingAccount = await GetAccountBySalesforceId(model.ObjectId);
        if (existingAccount == null) // Account doesn't exist, return from this action with an error
        {
            error = $"Account with Salesforce ID {model.ObjectId} does not exist in OSS.";
            await LogAction(transaction, SalesforceTransactionAction.UpdateAccountInOss, ActionObjectType.Account, StatusType.Error, null, error);
            return new Tuple<AccountV2, string>(null, error);
        }

        // Get the user from OSS system or default to system user when not found
        User existingUser = await _usersClient.GetUserByEmail(model.UserName) ?? _systemUser;

        var account = await RemapSalesforceAccountToOssAccount(model, sfModel);
        try
        {
            var updated = await _accountsClient.UpdateAccount(existingAccount.Id.GetValueOrDefault(), account);
            if (!string.IsNullOrEmpty(updated.Item2))
            {
                error = $"There was an error updating the account in OSS: {updated.Item2}";
                await LogAction(transaction, SalesforceTransactionAction.UpdateAccountInOss, ActionObjectType.Account, StatusType.Error, null, error);
                return new Tuple<AccountV2, string>(null, error);
            }
            await LogAction(transaction, SalesforceTransactionAction.UpdateAccountInOss, ActionObjectType.Account, StatusType.Successful, updated.Item1.Id.ToString());
            return new Tuple<AccountV2, string>(updated.Item1, string.Empty);
        }
        catch (Exception ex)
        {
            error = $"There was an error calling the OSS Accounts service: {ex.Message}";
            await LogAction(transaction, SalesforceTransactionAction.UpdateAccountInOss, ActionObjectType.Account, StatusType.Error, null, error);
            return new Tuple<AccountV2, string>(null, error);
        }
    }

    public async Task<Tuple<AccountV2, string>> UpdateChildAccount(AccountV2 account, string userName, SalesforceActionTransaction transaction)
    {
        await LogAction(transaction, SalesforceTransactionAction.UpdateChildAccountInOss, ActionObjectType.Account, StatusType.Started);
        string error = null;

        var existingAccount = await GetAccountBySalesforceId(account.SalesforceAccountId);
        if (existingAccount == null) // Account doesn't exist, return from this action with an error
        {
            error = $"Account with OSS ID {account.Id} does not exist in OSS.";
            return new Tuple<AccountV2, string>(null, error);
        }
        
        try
        {
            var updated = await _accountsClient.UpdateAccount(existingAccount.Id.GetValueOrDefault(), account);
            if (!string.IsNullOrEmpty(updated.Item2))
            {
                error = $"There was an error updating the account in OSS: {updated.Item2}";
                if (transaction != null) await LogAction(transaction, SalesforceTransactionAction.UpdateChildAccountInOss, ActionObjectType.Account, StatusType.Error, existingAccount.Id.ToString(), error);
                return new Tuple<AccountV2, string>(null, error);
            }
            await LogAction(transaction, SalesforceTransactionAction.UpdateChildAccountInOss, ActionObjectType.Account, StatusType.Successful, updated.Item1.Id.ToString());
            return new Tuple<AccountV2, string>(updated.Item1, string.Empty);
        }
        catch (Exception ex)
        {
            error = $"There was an error calling the OSS Accounts service: {ex.Message}";
            if (transaction != null) await LogAction(transaction, SalesforceTransactionAction.UpdateChildAccountInOss, ActionObjectType.Account, StatusType.Error, existingAccount.Id.ToString(), error);
            return new Tuple<AccountV2, string>(null, error);
        }
    }

    public async Task<Tuple<AccountV2, string>> UpdateAccountOracleId(SalesforceAccountModel model, string oracleId, SalesforceActionTransaction transaction)
    {
        await LogAction(transaction, SalesforceTransactionAction.UpdateAccountOracleIdInOss, ActionObjectType.Account, StatusType.Started);
        string error = null;

        var existingAccount = await GetAccountBySalesforceId(model.ObjectId);
        if (existingAccount == null) // Account doesn't exist, return from this action with an error
        {
            error = $"Account with Salesforce ID {model.ObjectId} does not exist in OSS.";
            await LogAction(transaction, SalesforceTransactionAction.UpdateAccountOracleIdInOss, ActionObjectType.Account, StatusType.Error, null, error);
            return new Tuple<AccountV2, string>(null, error);
        }
        // Get the user from OSS system
        User existingUser = null;
        if (!string.IsNullOrEmpty(model.UserName))
        {
            existingUser = await _usersClient.GetUserByEmail(model.UserName);
        }
        if (existingUser == null) existingUser = _systemUser;

        try
        {
            // make the update to the property
            existingAccount.OracleAccountId = oracleId;

            var updated = await _accountsClient.UpdateAccount(existingAccount.Id.GetValueOrDefault(), existingAccount);
            if (!string.IsNullOrEmpty(updated.Item2))
            {
                error = $"There was an error updating the account oracle Id in OSS: {updated.Item2}";
                await LogAction(transaction, SalesforceTransactionAction.UpdateAccountOracleIdInOss, ActionObjectType.Account, StatusType.Error, null, error);
                return new Tuple<AccountV2, string>(null, error);
            }
            await LogAction(transaction, SalesforceTransactionAction.UpdateAccountOracleIdInOss, ActionObjectType.Account, StatusType.Successful, updated.Item1.Id.ToString());
            return new Tuple<AccountV2, string>(updated.Item1, string.Empty);
        }
        catch (Exception ex)
        {
            error = $"There was an error calling the OSS Accounts service: {ex.Message}";
            await LogAction(transaction, SalesforceTransactionAction.UpdateAccountOracleIdInOss, ActionObjectType.Account, StatusType.Error, null, error);
            return new Tuple<AccountV2, string>(null, error);
        }
    }

    public async virtual Task<AccountV2> GetAccountBySalesforceId(string salesforceId)
    {
        return (await _accountsClient.GetAccountsByManySalesforceIds(new List<string> { salesforceId })).FirstOrDefault();
    }

    public async virtual Task<List<AccountV2>> GetAccountsByManySalesforceIds(List<string> salesforceIds)
    {
        return await _accountsClient.GetAccountsByManySalesforceIds(salesforceIds);
    }

    public async virtual Task<string> SyncAccountsToOSS(List<AccountV2> accounts)
    {
        return await _accountsClient.SyncAccountsFromSalesforce(accounts);
    }

    public async virtual Task<AccountV2> RemapSalesforceAccountToOssAccount(
        SalesforceAccountModel model,
        SalesforceAccountObjectModel sfModel,
        string? oracleAccountId = null,
        IEnumerable<SalesforceContactObjectModel>? allContacts = null,
        IEnumerable<AccountV2> allOssAccounts = null
        )
    {
        var account = new AccountV2
        {
            Id = string.IsNullOrEmpty(sfModel.KSN_Acct_ID__c) ? Guid.NewGuid() : Guid.Parse(sfModel.KSN_Acct_ID__c),
            SalesforceAccountId = sfModel.Id,
            Enabled = !sfModel.Inactive__c ?? true,
            Name = sfModel.Name,
            Origin = CreatedOriginEnum.SF,
            OracleAccountId = oracleAccountId,
            Parent = new ParentAccount
            {
                Id = _config.GetValue<Guid>("KymetaAccountId"),
                Name = "Kymeta Corporation"
            },
            AccountSubType = sfModel.Sub_Type__c,
            RelationshipType = sfModel.Type_of_Company__c,
            Configurator = new ConfiguratorAccess
            {
                // Configurator Properties
                CommercialPriceBook = sfModel.Pricebook__c?.Contains("CPB"),
                MilitaryPriceBook = sfModel.Pricebook__c?.Contains("MPB"),
                ConfiguratorVisible = sfModel.EB_Configurator_Visible__c,
                DiscountTier = GetDiscountTierFromSalesforceAPIValue(sfModel.Volume_Tier__c),
                MsrpPricesVisible = sfModel.EB_Configurator_Pricing_MSRP_Visible__c,
                WholesalePricesVisible = sfModel.EB_Configurator_Pricing_WS_Visible__c,
            },
        };
        // Overwrite the default Kymeta ID if the parent Id is present
        if (!string.IsNullOrEmpty(sfModel.ParentId))
        {
            // pull from list of available first
            AccountV2 parentAccount = allOssAccounts?.FirstOrDefault(a => a.SalesforceAccountId == sfModel.ParentId);
            // if null, pull from OSS
            if (parentAccount == null) parentAccount = (await _accountsClient.GetAccountsByManySalesforceIds(new List<string> { sfModel.ParentId }))?.FirstOrDefault();
            // if not null, bind
            if (parentAccount != null)
            {
                account.Parent.Id = parentAccount.Id;
                account.Parent.Name = parentAccount.Name;
            }
        }
        // Add a primary contact
        if (!string.IsNullOrEmpty(sfModel.Primary_Contact__c))
        {
            // pull from list if available first
            SalesforceContactObjectModel contactFromSalesforce = allContacts?.FirstOrDefault(c => c.Id == sfModel.Primary_Contact__c);
            // if null, pull from SF
            if (contactFromSalesforce == null) contactFromSalesforce = await _sfClient.GetContactFromSalesforce(sfModel.Primary_Contact__c);
            // if not null, bind
            if (contactFromSalesforce != null)
            {
                account.PrimaryContact = new PrimaryContact
                {
                    Email = contactFromSalesforce.Email,
                    Name = contactFromSalesforce.Name,
                    Phone = contactFromSalesforce.Phone
                };
            }
        }

        return account;
    }

    public int? GetDiscountTierFromSalesforceAPIValue(string? salesforceApiValue)
    {
        if (string.IsNullOrEmpty(salesforceApiValue)) return 1;
        if (!salesforceApiValue.Contains("(")) return 1;
        string firstPartOfValue = salesforceApiValue.Split('(')[0];
        if (firstPartOfValue == null) return 1;
        var justTheDigit = new string(firstPartOfValue.Where(char.IsDigit).ToArray());
        if (justTheDigit.Length == 0) return 1;
        return Convert.ToInt32(justTheDigit);
    }

    public async virtual Task LogAction(SalesforceActionTransaction transaction, SalesforceTransactionAction action, ActionObjectType objectType, StatusType status, string? entityId = null, string? errorMessage = null)
    {
        var actionRecord = new SalesforceActionRecord
        {
            ObjectType = objectType,
            Action = action,
            Status = status,
            Timestamp = DateTime.UtcNow,
            ErrorMessage = errorMessage,
            EntityId = entityId
        };
        if (transaction.TransactionLog == null) transaction.TransactionLog = new List<SalesforceActionRecord>();
        transaction.TransactionLog?.Add(actionRecord);
        await _actionsRepository.AddTransactionRecord(transaction.Id, transaction.Object.ToString() ?? "Unknown", actionRecord);
    }

    public async virtual Task<Tuple<bool, List<AccountV2>?, string>> UpdateChildAccounts(AccountV2 existingAccount, SalesforceAccountModel model, SalesforceActionTransaction salesforceTransaction)
    {
        // fetch the child Account metadata from OSS
        var childAccountSalesforceIds = model.ChildAccounts.Select(ca => ca.ObjectId).ToList();
        var childAccounts = await GetAccountsByManySalesforceIds(childAccountSalesforceIds);

        List<Task<Tuple<AccountV2, string>>> updateAccountTasks = new();
        // iterate the childAccounts & check to see that OSS parent references are pointed to ossAccountId
        for (int i = 0; i < childAccounts.Count; i++)
        {
            var childAccount = childAccounts[i];
            if (childAccount.Parent == null) childAccount.Parent = new ParentAccount();
            if (childAccount.Parent?.Id != existingAccount.Id)
            {
                // if parent reference is not correct, update child account
                childAccount.Parent.Id = existingAccount.Id;
                // add to list of Tasks to update children in parallel 
                updateAccountTasks.Add(UpdateChildAccount(childAccount, model.UserName, salesforceTransaction)); // need a method that only updates account and doesn't have context of the transaction
            }
        }

        List<AccountV2> updatedChildrenAccounts = new();
        Tuple<bool, List<AccountV2>?, string> result = new(true, updatedChildrenAccounts, null);
        // check to see if we need to update any children
        if (updateAccountTasks.Any())
        {
            // execute requests to create Locations in async fashion
            await Task.WhenAll(updateAccountTasks);
            // get the response data
            var updateChildAccountResults = updateAccountTasks.Select(t => t.Result).ToList();
            for (int i = 0; i < updateChildAccountResults.Count; i++)
            {
                // fetch child update result
                var childUpdateResult = updateChildAccountResults[i];
                if (childUpdateResult.Item1 == null)
                {
                    // something went wrong updating a child, return the error message
                    return new Tuple<bool, List<AccountV2>?, string>(false, null, childUpdateResult.Item2);
                }
                updatedChildrenAccounts.Add(childUpdateResult.Item1);
            }
        }

        // everything was updated as we intend, return success
        return new Tuple<bool, List<AccountV2>?, string>(true, updatedChildrenAccounts, null);
    }
}