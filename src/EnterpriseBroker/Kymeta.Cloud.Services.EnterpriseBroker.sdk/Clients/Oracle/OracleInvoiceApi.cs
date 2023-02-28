using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Invoice;
using Kymeta.Cloud.Services.Toolbox.Extensions;
using Kymeta.Cloud.Services.Toolbox.Rest;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients.Oracle;

public class OracleInvoiceApi
{
    private readonly HttpClient _client;
    private readonly ILogger<OracleInvoiceApi> _logger;

    public OracleInvoiceApi(HttpClient client, ILogger<OracleInvoiceApi> logger)
    {
        _client = client.NotNull();
        _logger = logger.NotNull();
    }

    public async Task<OracleInvoiceHeaderModel?> FindInvoiceByDeliveryName(IReadOnlyList<string> fullfillmentIds, CancellationToken token = default) => await new RestClient(_client)
        .SetPath($"receivablesInvoices?q=CreationDate>{DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd")}&expand=receivablesInvoiceLines.receivablesInvoiceTransactionDFF")
        .SetLogger(_logger)
        .GetAsync(token)
        .GetRequiredContent<OracleItemsResponse<OracleInvoiceHeaderModel>>()
        .FuncAsync(async x =>
        {
            OracleItemsResponse<OracleInvoiceHeaderModel> model = await (x);

            return model.Items
                .Where(x1 => x1.ReceivablesInvoiceLines.Any(x2 => x2.ReceivablesInvoiceLineTransactionDFF.Any(x3 => fullfillmentIds.Any(y => x3.DeliveryName.EqualsIgnoreCase(y)))))
               .FirstOrDefault();
        });

    public async Task<OracleCreateInvoiceResponseModel> Create(OracleCreateInvoiceModel request, CancellationToken token = default) => await new RestClient(_client)
        .SetPath($"receivablesInvoices")
        .SetLogger(_logger)
        .SetContent(request)
        .PostAsync(token)
        .GetRequiredContent<OracleCreateInvoiceResponseModel>();
}

