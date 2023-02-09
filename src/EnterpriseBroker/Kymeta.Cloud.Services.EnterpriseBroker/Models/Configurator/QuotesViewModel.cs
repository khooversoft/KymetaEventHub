namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Configurator
{
    public class QuoteRequestViewModel
    {
        public Guid? Id { get; set; }
        public int? OrderId { get; set; }
        public DateTime? CreatedOn { get; set; } 

        public string? Name { get; set; }
        public string? Company { get; set; }
        public string? Email { get; set; }
        public string? Notes { get; set; }
        public string? ShippingCountry { get; set; }
        #region Pricing Totals
        public long? MsrpTotal { get; set; }
        public long? MsrpMonthlyTotal { get; set; }
        public long? MsrpUpfrontTotal { get; set; }
        public long? WholesaleTotal { get; set; }
        public long? WholesaleMonthlyTotal { get; set; }
        public long? WholesaleUpfrontTotal { get; set; }
        #endregion
        #region Pricing Visibility
        public string? DiscountTier { get; set; }
        public bool? ShowPricing { get; set; }
        public bool? ShowWholesalePricing { get; set; }
        #endregion
        public List<QuoteRequestOrderItem>? Order { get; set; }
        public List<QuoteRequestConnectivityItem>? Connectivity { get; set; }
        public List<QuoteRequestFinancingItem>? Financing { get; set; }
    }

    public class QuoteRequestFinancingItem
    {
        public string? Duration { get; set; }
        public int? BundleId { get; set; }
    }

    public class QuoteRequestOrderItem
    {
        public string? Description { get; set; }
        public string? Type { get; set; }
        public string? KPC { get; set; }
        public string? UnitPriceMsrp { get; set; }
        public string? UnitPriceWholesale { get; set; }
        public string? TotalPriceMsrp { get; set; }
        public string? TotalPriceWholesale { get; set; }
        public int? BundleId { get; set; }
        public int? Quantity { get; set; }
    }

    public class QuoteRequestConnectivityItem
    {
        public string? KPC { get; set; }
        public int? DurationInMonths { get; set; }
        public string? UnitPriceMsrp { get; set; }
        public string? UnitPriceWholesale { get; set; }
        public string? Description { get; set; }
        public int? BundleId { get; set; }
    }
}

