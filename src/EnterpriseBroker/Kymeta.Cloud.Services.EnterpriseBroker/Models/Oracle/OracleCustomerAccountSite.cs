namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle
{
    public class OracleCustomerAccountSite
    {
        public string? PartyId { get; set; }
        public ulong? PartySiteId { get; set; }
        public string? PartyNumber { get; set; }
        public string? OrigSystemReference { get; set; }
        public string? SetId { get; set; }
        public List<OracleCustomerAccountSiteUse>? SiteUses { get; set; }
    }

    public class OracleCustomerAccountSiteUse
    {
        public string? SiteUseCode { get; set; }
    }
}
