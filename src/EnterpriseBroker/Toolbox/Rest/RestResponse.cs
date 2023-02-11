using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.Toolbox.Rest;

public record RestResponse
{
    public HttpResponseMessage HttpResponseMessage { get; init; } = null!;
    public string? Content { get; init; }
    public ILogger? Logger { get; init; }
}

