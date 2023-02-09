using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce;

public class SalesforceActionTransaction
{
    /// <summary>
    /// Event Id
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; set; }
    /// <summary>
    /// Timestamp when the request occurred.
    /// </summary>
    [JsonProperty("createdOn")]
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// User name (email) of the Salesforce user performing the action
    /// </summary>
    [JsonProperty("userName")]
    public string? UserName { get; set; }
    /// <summary>
    /// Object type (ie. Account, Contact)
    /// </summary>
    [JsonProperty("object")]
    [JsonConverter(typeof(StringEnumConverter))]
    public ActionObjectType? Object { get; set; }
    /// <summary>
    /// Id of the Salesforce object
    /// </summary>
    [JsonProperty("objectId")]
    public string? ObjectId { get; set; }
    /// <summary>
    /// Name of the Salesforce Object
    /// </summary>
    [JsonProperty("objectName")]
    public string? ObjectName { get; set; }
    /// <summary>
    /// Datetime the EnterpriseAction record was last updated on
    /// </summary>
    [JsonProperty("lastUpdatedOn")]
    public DateTime? LastUpdatedOn { get; set; }
    /// <summary>
    /// Body of the request that came in. Can be null.
    /// </summary>
    [JsonProperty("serializedObjectValues")]
    public string? SerializedObjectValues { get; set; }
    /// <summary>
    /// Log of actions in this transaction
    /// </summary>
    [JsonProperty("transactionLog")]
    public List<SalesforceActionRecord>? TransactionLog { get; set; }

    public UnifiedResponse? Response { get; set; }

    [JsonProperty("ossStatus")]
    public StatusType? OssStatus => string.IsNullOrEmpty(Response?.OSSStatus.ToString()) ? StatusType.Started : Response.OSSStatus;

    [JsonProperty("oracleStatus")]
    public StatusType? OracleStatus => string.IsNullOrEmpty(Response?.OracleStatus.ToString()) ? StatusType.Started : Response.OracleStatus;
}

public class SalesforceActionRecord
{
    [JsonConverter(typeof(StringEnumConverter))]
    public SalesforceTransactionAction Action { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public StatusType Status { get; set; }
    public ActionObjectType ObjectType { get; set; }
    public DateTime? Timestamp { get; set; }
    public string? ErrorMessage { get; set; }
    public string? EntityId { get; set; }
}

public enum SalesforceTransactionAction
{
    // Default
    Default,
    // Gets
    GetOrganizationInOracleBySFID,
    GetCustomerAccountBySFID,
    GetCustomerProfileBySFID,
    GetLocationBySalesforceId,
    GetPersonBySalesforceId,
    // Create Account
    CreateAccountInOss,
    CreateOrganizationInOracle,
    CreateCustomerAccountInOracle,
    CreateCustomerProfileInOracle,
    // Create Address
    CreateLocationInOracle,
    CreatePartySiteInOracle,
    UpdatePartySiteInOracle,
    CreateCustomerAccountSiteInOracle,
    // Update Account
    UpdateAccountInOss,
    UpdateChildAccountInOss,
    UpdateAccountOracleIdInOss,
    UpdateOrganizationInOracle,
    UpdateCustomerAccountInOracle,
    UpdateCustomerProfileInOracle,
    // Update Address
    UpdateAddressInOss,
    UpdateLocationInOracle,
    UpdateCustomerAccountSiteInOracle,
    // Create Contact
    CreatePersonInOracle,
    CreateCustomerAccountContactInOracle,
    // Update Contact
    UpdatePersonInOracle,
    UpdateCustomerContactContactInOracle,
    // Validation
    ValidateBusinessUnit
}

public enum ActionObjectType
{
    Account,
    Contact,
    Address
}

public enum StatusType
{
    Skipped,
    Started,
    Successful,
    Error
}
