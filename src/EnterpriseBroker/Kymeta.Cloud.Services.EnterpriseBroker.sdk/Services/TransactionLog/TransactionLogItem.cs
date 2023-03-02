namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services.TransactionLog;

public record TransactionLogItem
{
    public string InstanceId { get; init; } = null!;
    public string Method { get; init; } = null!;
    public long ReplayId { get; init; }
    public string TypeName { get; init; } = null!;
    public string SubjectJson { get; init; } = null!;
}