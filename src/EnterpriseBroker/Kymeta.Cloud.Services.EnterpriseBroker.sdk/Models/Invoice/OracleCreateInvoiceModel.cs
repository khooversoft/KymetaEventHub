using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Invoice;

public record OracleCreateInvoiceModel
{
    public string TransactionType { get; } = "Invoice";
    public long? CustomerTransactionId { get; init; }
    public string TransactionNumber { get; init; } = null!;
    public string BillToCustomerNumber { get; init; } = null!;
    public string ShipToCustomerNumber { get; init; } = null!;
    public string? ShipToSite { get; init; }
    public string? BillingDate { get; init; }
    public string PaymentTerms { get; init; } = null!; 
    public string? TransactionDate { get; init; }
    public string BusinessUnit { get; init; } = null!; 
    public string InvoiceCurrencyCode { get; init; } = null!; 
    public string CrossReference { get; init; } = null!; 
    public string? DueDate { get; init; }
    public string TransactionSource { get; init; } = null!; 
    public string InternalNotes { get; init; } = null!; 
    public string Comments { get; init; } = null!;

    [JsonPropertyName("receivablesInvoiceLines")]
    public IReadOnlyList<OracleCreateInvoiceLineModel> InvoiceLines { get; init; } = Array.Empty<OracleCreateInvoiceLineModel>();
}


public record OracleCreateInvoiceLineModel
{
    public long LineNumber { get; init; }
    public string Description { get; init; } = null!; 

    // No variable in Oracle Invoice ???
    public string SalesOrder { get; init; } = null!;
    public float Quantity { get; init; }

    // Is this "Unit Selling Price" ???
    public decimal UnitSellingPrice { get; init; }

    // There is no ItemNumber in oracle invoice ???
    public string ItemNumber { get; init; } = null!;
}