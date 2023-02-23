using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Invoice;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.InvoiceCreate.Model;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.SalesOrder.Activities;
using Kymeta.Cloud.Services.Toolbox.Extensions;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.InvoiceCreate.Activities;

public class Step1_GetInvoiceLineItemsActivity : AsyncTaskActivity<Event_InvoiceCreateModel, SalesforceInvoiceLineModel>
{
    private readonly ITransactionLoggingService _transLog;
    private readonly ILogger<Step2_GetSalesOrderLinesActivity> _logger;
    public Step1_GetInvoiceLineItemsActivity(ITransactionLoggingService transLog, ILogger<Step2_GetSalesOrderLinesActivity> logger)
    {
        _transLog = transLog.NotNull();
        _logger = logger.NotNull();
    }

    protected override Task<SalesforceInvoiceLineModel> ExecuteAsync(TaskContext context, Event_InvoiceCreateModel input)
    {
        _transLog.Add(this.GetMethodName(), context.OrchestrationInstance.InstanceId, input);

        var result = new SalesforceInvoiceLineModel();

        _transLog.Add(this.GetMethodName(), context.OrchestrationInstance.InstanceId, result);
        return Task.FromResult(result);
    }
}