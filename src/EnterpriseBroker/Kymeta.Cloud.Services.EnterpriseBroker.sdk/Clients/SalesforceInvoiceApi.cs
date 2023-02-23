using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Invoice;
using Kymeta.Cloud.Services.Toolbox.Rest;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients;

public class SalesforceInvoiceApi
{
    private readonly HttpClient _client;
    private readonly ILogger<SalesforceInvoiceApi> _logger;

    public SalesforceInvoiceApi(HttpClient client, ILogger<SalesforceInvoiceApi> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<SalesforceInvoiceLineModel> SearchInvoice(string invoiceId, CancellationToken token) => await new RestClient(_client)
        .SetPath($"query?q=select FIELDS(ALL) from blng__InvoiceLine__c where blng__Invoice__c={invoiceId} LIMIT 200")
        .SetLogger(_logger)
        .GetAsync()
        .GetRequiredContent<SalesforceInvoiceLineModel>();
}
