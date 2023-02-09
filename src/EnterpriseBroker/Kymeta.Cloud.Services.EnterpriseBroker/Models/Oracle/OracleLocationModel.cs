namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle;

public class OracleLocationModel
{
    public ulong? LocationId { get; set; }
    public string OrigSystemReference { get; set; }
    public string Address1 { get; set; }
    public string Address2 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }
}