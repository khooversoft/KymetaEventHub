using System.ComponentModel.DataAnnotations;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce;

public class SalesforceActionObject
{
    /// <summary>
    /// Who initiated the request. This is usually an e-mail.
    /// </summary>
    public string? UserName { get; set; }
    /// <summary>
    /// Origin URI of the request.
    /// </summary>
    public string? EnterpriseOriginUri { get; set; }
    /// <summary>
    /// Id of the Salesforce Object.
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    public string? ObjectId { get; set; }
    public bool? SyncToOss { get; set; }
    public bool? SyncToOracle { get; set; }
}
