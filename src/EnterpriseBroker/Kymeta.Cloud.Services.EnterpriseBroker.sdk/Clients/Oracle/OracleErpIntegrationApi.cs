using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Invoice;
using Kymeta.Cloud.Services.Toolbox.Rest;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients.Oracle;

public class OracleErpIntegrationApi
{
    private readonly HttpClient _client;
    private readonly ILogger<OracleErpIntegrationApi> _logger;

    public OracleErpIntegrationApi(HttpClient client, ILogger<OracleErpIntegrationApi> logger)
    {
        _client = client.NotNull();
        _logger = logger.NotNull();
    }

    public async Task<OracleErpIntegrationResponseModel> PostRequest(OrcaleErpIntegrationsRequestModel request, CancellationToken token = default) => await new RestClient(_client)
        .SetPath("erpintegrations")
        .SetLogger(_logger)
        .SetContent(request)
        .PostAsync(token)
        .GetRequiredContent<OracleErpIntegrationResponseModel>();
}
