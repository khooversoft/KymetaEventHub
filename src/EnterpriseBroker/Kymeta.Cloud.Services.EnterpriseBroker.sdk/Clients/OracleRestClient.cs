using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients
{
    public interface IOracleRestClient
    {
        Task<OracleResponse<CreateOrderResponse>> CreateOrder(OracleCreateOrder newOrder, CancellationToken cancellationToken);
        Task<OracleResponse<GetOrderResponse>> GetOrder(string? orderKey, CancellationToken cancellationToken);
        Task<OracleResponse<UpdateOrderResponse>> UpdateOrder(OracleUpdateOrder newOrder, CancellationToken cancellationToken);
    }

    public class OracleRestClient : IOracleRestClient
    {
        private const string RequestUri = "fscmRestApi/resources/11.13.18.05/salesOrdersForOrderHub";
        private readonly HttpClient _client;
        public OracleRestClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<OracleResponse<CreateOrderResponse>> CreateOrder(OracleCreateOrder newOrder, CancellationToken cancellationToken)
        {

            HttpResponseMessage response = await _client.PostAsync(RequestUri, SerializeToJsonString(newOrder), cancellationToken);

            return await response.ProcessResponseFromOracle<CreateOrderResponse>(cancellationToken);
        }

        private static StringContent SerializeToJsonString(OracleCreateOrder newOrder)
        {
            var serialized = JsonSerializer.Serialize(newOrder, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return new StringContent(serialized, Encoding.UTF8, "application/json");
        }

        public Task<OracleResponse<UpdateOrderResponse>> UpdateOrder(OracleUpdateOrder newOrder, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<OracleResponse<GetOrderResponse>> GetOrder(string? orderKey, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await _client.GetAsync($"{RequestUri}?q=OrderNumber={orderKey}", cancellationToken);
            return await response.ProcessResponseFromOracle<GetOrderResponse>(cancellationToken);
        }
    }
}
