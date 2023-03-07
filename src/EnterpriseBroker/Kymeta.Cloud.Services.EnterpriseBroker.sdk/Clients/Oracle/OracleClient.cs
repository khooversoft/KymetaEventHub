using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients.Oracle;

public class OracleClient
{
    public OracleClient(HttpClient client, ILoggerFactory loggerFactory)
    {
        client.NotNull();
        loggerFactory.NotNull();

        Integration = new OracleErpIntegrationApi(client, loggerFactory.CreateLogger<OracleErpIntegrationApi>());
        Invoice = new OracleInvoiceApi(client, loggerFactory.CreateLogger<OracleInvoiceApi>());
    }

    public OracleErpIntegrationApi Integration { get; }
    public OracleInvoiceApi Invoice { get; }
}
