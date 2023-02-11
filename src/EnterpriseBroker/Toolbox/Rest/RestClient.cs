using System.Text;
using Kymeta.Cloud.Services.Toolbox.Extensions;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;

namespace Kymeta.Cloud.Services.Toolbox.Rest;

/// <summary>
/// REST Client provides a builder pattern for making a REST API call.
/// 
/// Note: if the Absolute URI is specified, this URI is used and not build
/// process is not used.
/// </summary>
public class RestClient
{
    private readonly HttpClient _client = null!;

    public RestClient(HttpClient client) => _client = client.NotNull();

    public RestClient(RestClient restClient)
    {
        restClient.NotNull();
        _client = restClient._client;
        Logger = restClient.Logger;
    }

    public string? Path { get; private set; }

    public object? Content { get; private set; }

    public ILogger? Logger { get; private set; }
    public bool EnsureSuccessStatusCode { get; private set; } = true;

    public RestClient Clear()
    {
        Path = null;
        Content = null;

        return this;
    }

    public RestClient SetPath(string path) => this.Action(x => x.Path = path);
    public RestClient SetContent(HttpContent content) => this.Action(x => Content = content);
    public RestClient SetContent<T>(T value, bool required = true)
    {
        if (required) value.NotNull();

        Content = value;
        return this;
    }

    public RestClient SetLogger(ILogger logger) => this.Action(x => x.Logger = logger.NotNull());
    public RestClient SetEnsureSuccessStatusCode(bool state) => this.Action(x => x.EnsureSuccessStatusCode = state);

    public async Task<RestResponse> SendAsync(HttpRequestMessage requestMessage, CancellationToken token = default)
    {
        using var ls = Logger?.LogEntryExit();
        Logger?.LogTrace("Calling {uri}, method={method}, content={request}", requestMessage.RequestUri?.ToString(), requestMessage.Method, await requestMessage.GetContent());

        HttpResponseMessage response = await _client.SendAsync(requestMessage.NotNull(), token);
        string content = await response.Content.ReadAsStringAsync();

        Logger?.Log(
            response.IsSuccessStatusCode ? LogLevel.Information : LogLevel.Error,
            "Response from {uri}, method={method}, StatusCode={statusCode}, Content={content}",
            requestMessage.RequestUri?.ToString(),
            requestMessage.Method,
            response.StatusCode,
            (content.ToNullIfEmpty() ?? "<no content>").Truncate(100)
            );

        var result = new RestResponse
        {
            HttpResponseMessage = response,
            Content = content,
            Logger = Logger,
        };

        if (EnsureSuccessStatusCode) response.EnsureSuccessStatusCode();
        return result;
    }

    public Task<RestResponse> GetAsync(CancellationToken token = default) => SendAsync(BuildRequestMessage(HttpMethod.Get), token);

    public Task<RestResponse> DeleteAsync(CancellationToken token = default) => SendAsync(BuildRequestMessage(HttpMethod.Delete), token);

    public Task<RestResponse> PostAsync(CancellationToken token = default) => SendAsync(BuildRequestMessage(HttpMethod.Post), token);

    public Task<RestResponse> PutAsync(CancellationToken token = default) => SendAsync(BuildRequestMessage(HttpMethod.Put), token);


    private HttpRequestMessage BuildRequestMessage(HttpMethod method) => new HttpRequestMessage(method, EscapeQuery(Path))
    {
        Content = Content switch
        {
            null => null,
            HttpContent v => v,

            var v => new StringContent(Json.Default.SerializePascal(v), Encoding.UTF8, "application/json")
        },
    };

    private string EscapeQuery(string? path) => path
        .NotEmpty()
        .Split('?', StringSplitOptions.RemoveEmptyEntries) switch
    {
        string[] v when v.Length == 1 => v[0],
        string[] v when v.Length == 2 => v[0] + "?" + EscapeLine(v[1]),

        _ => throw new InvalidOperationException(),
    };

    private string EscapeLine(string line) => line
        .Func(x => x.Replace("#", "%23"))
        .Func(x => x.Replace("+", "%2B"))
        .Func(x => x.Replace(" ", "%20"));
}
