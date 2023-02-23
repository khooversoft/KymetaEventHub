using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Invoice;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.InvoiceCreate.Model;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.SalesOrder.Activities;
using Kymeta.Cloud.Services.Toolbox.Extensions;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.InvoiceCreate.Activities;

public class Step2_CreateHardwareInvoiceActivity : AsyncTaskActivity<SalesforceInvoiceLineModel, CreatedInvoiceModel>
{
    private readonly ITransactionLoggingService _transLog;
    private readonly ILogger<Step2_GetSalesOrderLinesActivity> _logger;
    public Step2_CreateHardwareInvoiceActivity(ITransactionLoggingService transLog, ILogger<Step2_GetSalesOrderLinesActivity> logger)
    {
        _transLog = transLog.NotNull();
        _logger = logger.NotNull();
    }

    protected override Task<CreatedInvoiceModel> ExecuteAsync(TaskContext context, SalesforceInvoiceLineModel input)
    {
        _transLog.Add(this.GetMethodName(), context.OrchestrationInstance.InstanceId, input);

        var result = new CreatedInvoiceModel();

        _transLog.Add(this.GetMethodName(), context.OrchestrationInstance.InstanceId, result);
        return Task.FromResult(result);
    }
}