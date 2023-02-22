
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients;

public interface ISalesforceRestApi
{
    Task<IEnumerable<OrderProduct>> GetOrderProducts(string orderNumber, CancellationToken cancellationToken);
}

public class SalesforceRestApi : ISalesforceRestApi
{
    private readonly HttpClient _client;
    private readonly ILogger<ISalesforceRestApi> _logger;

    public SalesforceRestApi(HttpClient client, ILogger<ISalesforceRestApi> logger)
    {
        _client = client.NotNull();
        _logger = logger.NotNull();
    }

    public async Task<IEnumerable<OrderProduct>> GetOrderProducts(string orderKey, CancellationToken cancellationToken)
    {

        HttpResponseMessage response = await _client.GetAsync($"/services/data/v56.0/query?q=select FIELDS(ALL) from OrderItem where orderId=’{orderKey}’ and sync_to_oracle__c=false LIMIT 200", cancellationToken);

        return new[] { new OrderProduct() };
    }
}
