using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Responses;

public class SalesforceProcessResponse
{
    /// <summary>
    /// Id of the Salesforce object which was created
    /// </summary>
    public string? SalesforceObjectId { get; set; }
    /// <summary>
    /// Time this process was completed on
    /// </summary>
    public DateTime CompletedOn { get; set; }
    /// <summary>
    /// Status of the OSS process
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public StatusType OSSStatus { get; set; }
    /// <summary>
    /// Status of the Oracle process
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public StatusType OracleStatus { get; set; }
    /// <summary>
    /// Any error messages from OSS
    /// </summary>
    public string? OSSErrorMessage { get; set; }
    /// <summary>
    /// Any error messages from Oracle
    /// </summary>
    public string? OracleErrorMessage { get; set; }
}

