using Kymeta.Cloud.Services.Toolbox.Extensions;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;

public record Step2_TestModel
{
    public string? SourceTransactionId { get; init; }
    public string? OrderKey { get; init; }
}
