using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kymeta.Cloud.Services.Toolbox.Extensions;
using Kymeta.Cloud.Services.Toolbox.Rest;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients.Salesforce;

public class SalesforceEventApi
{
    private readonly HttpClient _client;
    private readonly ILogger<SalesforceEventApi> _logger;

    public SalesforceEventApi(HttpClient client, ILogger<SalesforceEventApi> logger)
    {
        _client = client.NotNull();
        _logger = logger.NotNull();
    }

    public async Task<bool> SendEvent<T>(string eventName, T value, CancellationToken token) => await new RestClient(_client)
        .SetPath($"sobjects/{eventName.NotEmpty()}")
        .SetLogger(_logger)
        .SetContent(value)
        .PostAsync(token)
        .FuncAsync(async x => await x switch
        {
            RestResponse v when v.HttpResponseMessage.IsSuccessStatusCode => true,
            _ => false,
        });
}
