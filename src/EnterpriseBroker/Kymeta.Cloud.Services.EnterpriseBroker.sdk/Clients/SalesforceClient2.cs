using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kymeta.Cloud.Commons.Databases.Redis;
using Kymeta.Cloud.Services.Toolbox.Rest;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients;

public class SalesforceClient2
{
    private readonly HttpClient _httpClient;
    private readonly IRedisClient _redis;
    private readonly ILogger<SalesforceClient2> _logger;

    public SalesforceClient2(HttpClient httpClient, IRedisClient redis, ILogger<SalesforceClient2> logger)
    {
        _httpClient = httpClient.NotNull();
        _redis = redis.NotNull();
        _logger = logger.NotNull(); ;
    }

    public async Task<RestResponse> DoSomething(string addressId) => await new RestClient(_httpClient)
        .SetPath($"services/data/v53.0/sobjects/Address__c/{addressId}")
        .GetAsync(CancellationToken.None);
}
