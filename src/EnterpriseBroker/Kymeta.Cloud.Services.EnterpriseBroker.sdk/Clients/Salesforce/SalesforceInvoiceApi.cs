using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Invoice;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Salesforce;
using Kymeta.Cloud.Services.Toolbox.Rest;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients.Salesforce;

public class SalesforceInvoiceApi
{
    private readonly HttpClient _client;
    private readonly ILogger<SalesforceInvoiceApi> _logger;

    public SalesforceInvoiceApi(HttpClient client, ILogger<SalesforceInvoiceApi> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<SalesforceSearchResult<SalesforceInvoiceLineModel>> SearchLines(string invoiceId, CancellationToken token = default) => await new RestClient(_client)
        .SetPath($"query?q=select FIELDS(ALL) from blng__InvoiceLine__c where blng__Invoice__c='{invoiceId}'  LIMIT 200")
        .SetLogger(_logger)
        .GetAsync(token)
        .GetRequiredContent<SalesforceSearchResult<SalesforceInvoiceLineModel>>();

    public async Task Update(string invoiceId, SalesforceUpdateInvoiceRequestModel subject, CancellationToken token = default) => await new RestClient(_client)
        .SetPath($"sobjects/invoice/{invoiceId.NotEmpty()}")
        .SetLogger(_logger)
        .SetContent(subject)
        .PatchAsync(token);
}
