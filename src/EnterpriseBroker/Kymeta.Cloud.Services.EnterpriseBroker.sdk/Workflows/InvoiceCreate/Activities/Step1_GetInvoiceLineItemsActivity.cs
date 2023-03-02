using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients.Salesforce;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Invoice;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Salesforce;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services.TransactionLog;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.InvoiceCreate.Model;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.SalesOrder.Activities;
using Kymeta.Cloud.Services.Toolbox.Extensions;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.InvoiceCreate.Activities;

public class Step1_GetInvoiceLineItemsActivity : AsyncTaskActivity<Event_InvoiceCreateModel, IReadOnlyList<SalesforceInvoiceLineModel>>
{
    private readonly ITransactionLoggingService _transLog;
    private readonly ILogger<Step2_GetSalesOrderLinesActivity> _logger;
    private readonly SalesforceClient2 _client;

    public Step1_GetInvoiceLineItemsActivity(SalesforceClient2 client, ITransactionLoggingService transLog, ILogger<Step2_GetSalesOrderLinesActivity> logger)
    {
        _client = client.NotNull();
        _transLog = transLog.NotNull();
        _logger = logger.NotNull();
    }

    protected override async Task<IReadOnlyList<SalesforceInvoiceLineModel>> ExecuteAsync(TaskContext context, Event_InvoiceCreateModel input)
    {
        using var ls = _logger.LogEntryExit(message: $"InstanceId={context.OrchestrationInstance.InstanceId}");
        _transLog.Add(this.GetMethodName(), context.OrchestrationInstance.InstanceId, input);

        IReadOnlyList<SalesforceInvoiceLineModel> result = (await _client.Invoice.SearchLines(input.NEO_id__c)).Records.ToArray();
        _logger.LogInformation("Return search results, coun={count}", result.Count);

        _transLog.Add(this.GetMethodName(), context.OrchestrationInstance.InstanceId, result);
        return result;
    }
}