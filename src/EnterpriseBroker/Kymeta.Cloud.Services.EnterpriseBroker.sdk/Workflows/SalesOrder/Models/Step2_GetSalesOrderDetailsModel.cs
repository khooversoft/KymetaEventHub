using Kymeta.Cloud.Services.Toolbox.Extensions;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;

public record Step2_GetSalesOrderDetailsModel
{
    public string? SourceTransactionId { get; init; }
    public string? OrderKey { get; init; }
    public IReadOnlyList<SalesOrderLineModel> Lines { get; init; } = Array.Empty<SalesOrderLineModel>();
}

public record SalesOrderLineModel
{
    public string? SourceTransactionLineId { get; init; }
    public string? SourceTranscationScheduleId { get; init; }
    public long? OrderQuantity { get; init; }
}


public static class SalesOrderModelExtensions
{
    public static bool IsValid(this Step2_GetSalesOrderDetailsModel subject)
    {
        return subject != null &&
            subject.SourceTransactionId.IsNotEmpty() &&
            subject.OrderKey.IsNotEmpty() &&
            subject.Lines != null &&
            subject.Lines.All(x => x.IsValid());
    }

    public static bool IsValid(this SalesOrderLineModel subject)
    {
        return subject != null &&
            subject.SourceTransactionLineId.IsNotEmpty() &&
            subject.SourceTranscationScheduleId.IsNotEmpty() &&
            subject.OrderQuantity != null &&
            subject.OrderQuantity >= 0;
    }
}
