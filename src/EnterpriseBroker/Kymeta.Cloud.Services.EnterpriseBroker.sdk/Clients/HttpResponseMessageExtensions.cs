using Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.REST;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<OracleResponse<T>> ProcessResponseFromOracle<T>(this HttpResponseMessage response, CancellationToken cancellationToken) where T : IOracleResponsePayload
        {
            string content = await response.Content.ReadAsStringAsync(cancellationToken);

            return new OracleResponse<T>(response.StatusCode, response.ReasonPhrase, content);
        }
    }
}
