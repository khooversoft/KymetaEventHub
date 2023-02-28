using System.Text.Json.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Invoice;

public record OracleInvoiceHeaderModel
{
    public long CustomerTransactionId { get; init; }

    [JsonPropertyName("receivablesInvoiceLines")]
    public IReadOnlyList<OracleInvoiceItemModel> ReceivablesInvoiceLines { get; init; } = Array.Empty<OracleInvoiceItemModel>();
}

public record OracleInvoiceItemModel
{
    [JsonPropertyName("receivablesInvoiceLineTransactionDFF")]
    public IReadOnlyList<OracleInvoiceDFFModel> ReceivablesInvoiceLineTransactionDFF { get; init; } = Array.Empty<OracleInvoiceDFFModel>();
}

public record OracleInvoiceDFFModel
{
    // This is FulfillLineId
    [JsonPropertyName("_Delivery__Name")]
    public string? DeliveryName { get; init; }
}