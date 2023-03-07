using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Salesforce;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Shipping;
using Kymeta.Cloud.Services.Toolbox.Rest;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients.Salesforce;

public class SalesforceSalesOrderApi
{
    private readonly HttpClient _client;
    private readonly ILogger<SalesforceSalesOrderApi> _logger;

    public SalesforceSalesOrderApi(HttpClient client, ILogger<SalesforceSalesOrderApi> logger)
    {
        _client = client.NotNull();
        _logger = logger.NotNull();
    }

    public async Task<IEnumerable<OrderProduct>> GetOrderProducts(string orderKey, CancellationToken cancellationToken)
    {

        HttpResponseMessage response = await _client.GetAsync($"/services/data/v56.0/query?q=select FIELDS(ALL) from OrderItem where orderId=’{orderKey}’ and sync_to_oracle__c=false LIMIT 200", cancellationToken);

        return new[] { new OrderProduct() };
    }

    public async Task<SalesforceSearchResult<FullmentSearchResult>> Search(string fulfillmentId, CancellationToken token = default) => await new RestClient(_client)
        .SetPath($"query?q=select Id from orderItem where NEO_Oracle_Fulfillment_Id__c = '{fulfillmentId}' LIMIT 150")
        .SetLogger(_logger)
        .GetAsync(token)
        .GetRequiredContent<SalesforceSearchResult<FullmentSearchResult>>();

    public async Task UpdateOrderItem(string id, UpdateProductModel updateProductModel, CancellationToken token = default) => await new RestClient(_client)
        .SetPath($"sobjects/OrderItem/{id}")
        .SetLogger(_logger)
        .SetContent(updateProductModel)
        .PatchAsync(token);
}
