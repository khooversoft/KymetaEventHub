using System.Text.Json.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;

public record SalesorderEventModel
{
    public string? EventUuid { get; init; }

    [JsonPropertyName("replayId")]
    public long? ReplayId { get; init; }
    public string? EventApiName { get; init; }
}
