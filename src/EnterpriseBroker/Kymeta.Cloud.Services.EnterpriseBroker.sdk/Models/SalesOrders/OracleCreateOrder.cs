using System.Text.Json.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders
{
    public record OracleCreateOrder
    {
        public string? SourceTransactionNumber { get; set; }
        public string? SourceTransactionId { get; set; }
        public string? OrderKey { get; set; }
        public string? SourceTransactionSystem { get; set; }
        public string? BusinessUnitName { get; set; }
        public string? BuyingPartyNumber { get; set; }
        public string? BuyingPartyContactNumber { get; set; }
        public string? TransactionType { get; set; }
        public string? RequestedShipDate { get; set; }
        public string? PaymentTerms { get; set; }
        public string? TransactionalCurrencyCode { get; set; }
        public string? RequestingBusinessUnitName { get; set; }
        public bool FreezePriceFlag { get; set; }
        public bool FreezeShippingChargeFlag { get; set; }
        public bool FreezeTaxFlag { get; set; }
        public bool SubmittedFlag { get; set; }
        public string? SourceTransactionRevisionNumber { get; set; }
        [JsonPropertyName("billToCustomer")]
        public CustomerBill[] BillToCustomer { get; set; } = Array.Empty<CustomerBill>();
        [JsonPropertyName("shipToCustomer")]
        public CustomerShip[] ShipToCustomer { get; set; } = Array.Empty<CustomerShip>();
        [JsonPropertyName("lines")]
        public OrderLines[] Lines { get; set; } = Array.Empty<OrderLines>();
    }

    public record OrderLines
    {
        public string? SourceTransactionLineId { get; set; }
        public string? SourceTransactionLineNumber { get; set; }
        public string? SourceTransactionScheduleId { get; set; }
        public string? SourceScheduleNumber { get; set; }
        public string? TransactionCategoryCode { get; set; }
        public string? TransactionLineType { get; set; }
        public string? ProductNumber { get; set; }
        public string? OrderedQuantity { get; set; }
        public string? OrderedUOM { get; set; }
    }

    public record CustomerBill
    {
        public string? AccountNumber { get; set; }
        //public long ContactNumber { get; set; }
        //public string AddressId { get; set; }
    }

    public record CustomerShip
    {
        public string? PartyNumber { get; set; }
        //public long ContactNumber { get; set; }
        //public string AddressId { get; set; }
    }
}