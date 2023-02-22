namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Application;

public record OrchestrationConfiguration
{
    public IReadOnlyList<Type> TaskOrchestrations { get; init; } = Array.Empty<Type>();
    public IReadOnlyList<Type> TaskActivities { get; init; } = Array.Empty<Type>();
    public IDictionary<string, Type> ChannelMapToOrchestrations { get; init; } = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
}

