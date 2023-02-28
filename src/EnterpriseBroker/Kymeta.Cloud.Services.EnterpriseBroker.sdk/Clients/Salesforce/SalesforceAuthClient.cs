using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Application;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models;
using Kymeta.Cloud.Services.Toolbox.Extensions;
using Kymeta.Cloud.Services.Toolbox.Rest;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients.Salesforce;

public class SalesforceAuthClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SalesforceAuthClient> _logger;
    private readonly ServiceOption _serviceOption;
    private readonly IMemoryCache _memoryCache;

    public SalesforceAuthClient(HttpClient httpClient, ServiceOption serviceOption, IMemoryCache memoryCache, ILogger<SalesforceAuthClient> logger)
    {
        _httpClient = httpClient.NotNull();
        _serviceOption = serviceOption.NotNull();
        _memoryCache = memoryCache.NotNull();
        _logger = logger.NotNull();
    }

    public async Task<SalesforceAuthenticationResponse?> GetAuthToken(CancellationToken token, bool forceRefresh = false)
    {
        using var ls = _logger.LogEntryExit();
        _logger.LogInformation("Getting auth token from Salesforce");

        if (!forceRefresh && _memoryCache.TryGetValue(nameof(SalesforceAuthClient), out SalesforceAuthenticationResponse? fromCache))
        {
            return fromCache;
        }

        var result = await new RestClient(_httpClient)
            .SetPath("services/oauth2/token")
            .SetLogger(_logger)
            .SetContent(CreateContent())
            .PostAsync(token)
            .GetRequiredContent<SalesforceAuthenticationResponse>();

        _memoryCache.Set(nameof(SalesforceAuthClient), result, TimeSpan.FromHours(6));
        return result;
    }

    private HttpContent CreateContent() => new (string key, string value)[]
    {
        ( "grant_type", "password" ),
        ( "client_id", _serviceOption.Salesforce.ConnectedApp.ClientId ),
        ( "client_secret", _serviceOption.Salesforce.ConnectedApp.ClientSecret ),
        ( "username", _serviceOption.Salesforce.Username ),
        ( "password", _serviceOption.Salesforce.Password ),
    }
    .Select(x => new KeyValuePair<string, string>(x.key, x.value))
    .Func(x => new FormUrlEncodedContent(x));
}
