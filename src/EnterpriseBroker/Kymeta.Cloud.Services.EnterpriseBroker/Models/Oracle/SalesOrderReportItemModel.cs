using CsvHelper.Configuration;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle;

public class SalesOrderReportItemModel
{
    [Name("ACCOUNT_NUMBER")]
    public string? AccountNumber { get; set; }
    [Name("SOURCE_ORDER_NUMBER")]
    public string? SalesOrderNumber { get; set; }
    [Name("SERIAL_NOS")]
    [TypeConverter(typeof(DelimitedStringConverter))]
    public List<string>? SerialNumbers { get; set; }

    #region Unused Columns From Report
    //[Name("P_REPORT_TYPE")]
    //public string? ReportType { get; set; }
    //[Name("PARTY_NAME")]
    //public string? PartyName { get; set; }
    //[Name("REGISTRY_ID")]
    //public string? RegistryId { get; set; }
    //[Name("CUSTOMER_PO_NUMBER")]
    //public string? CustomerPoNumber { get; set; }
    //[Name("FULFILL_LINE_NUMBER")]
    //public string? FulfillLineNumber { get; set; }
    //[Name("SOURCE_LINE_NUMBER")]
    //public string? SourceLineNumber { get; set; }
    //[Name("FULFILLMENT_STATUS")]
    //public string? FulfillmentStatus { get; set; }
    //[Name("LINE_STATUS")]
    //public string? LineStatus { get; set; }
    //[Name("FULFILL_INV_ORGANIZATION")]
    //public string? FulfillInvOrganization { get; set; }
    //[Name("FULFILLED_QTY")]
    //public int? FulfilledQuantity { get; set; }
    //[Name("ORDERED_QTY")]
    //public int? OrderedQuantity { get; set; }
    //[Name("DELIVERY_NAME")]
    //public string? DeliveryName { get; set; }
    //[Name("REQUEST_SHIPMENT_DATE")]
    //public string? RequestShipmentDate { get; set; }
    //[Name("SCHEDULE_SHIPMENT_DATE")]
    //public string? ScheduleShipmentDate { get; set; }
    //[Name("ACTUAL_SHIPMENT_DATE")]
    //public string? ActualShipmentDate { get; set; }
    //[Name("FULFILLMENT_DATE")]
    //public string? FulfillmentDate { get; set; }
    //[Name("UNIT_SELLING_PRICE")]
    //public string? UnitSellingPrice { get; set; }
    //[Name("TOTAL_ORDERED_AMOUNT")]
    //public string? TotalOrderedAmount { get; set; }
    //[Name("ITEM_NUMBER")]
    //public string? ItemNumber { get; set; }
    //[Name("DESCRIPTION")]
    //public string? Description { get; set; }
    //[Name("TRACKING_NUMBER")]
    //public string? TrackingNumber { get; set; }
    //[Name("UNIQUE_LINE_ID")]
    //public string? UniqueLineId { get; set; }
    //[Name("SHIPPING_METHOD")]
    //public string? ShippingMethod { get; set; }
    //[Name("DELIVERED_SUBINVENTORY")]
    //public string? DeliveredSubInventory { get; set; }
    #endregion
}

public class DelimitedStringConverter : DefaultTypeConverter
{
    // convert from semicolon delimited string to a list of serials (string)
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        // return an empty list if the record has no text
        if (string.IsNullOrEmpty(text)) return new List<string>();
        // split items on semicolon
        return text.Split(';')
            .Select(x => x.Trim())
            .ToList();
    }
}