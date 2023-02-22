using Kymeta.Cloud.Services.Toolbox.Extensions;
using Kymeta.Cloud.Services.Toolbox.Tools;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models;

public record MessageEventContent
{
    public string Id { get; } = Guid.NewGuid().ToString();
    public DateTime Timestamp { get; } = DateTime.UtcNow;

    public string Channel { get; init; } = null!;
    public string ChannelId { get; init; } = null!;
    public long ReplayId { get; init; }
    public string Json { get; init; } = null!;
}


public static class MessageEventContentExtensions
{
    public static T? ToObject<T>(this MessageEventContent messageEventContent) where T : class => messageEventContent.NotNull().Json.ToObject<T>();
}