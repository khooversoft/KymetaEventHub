using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services.TransactionLog;
using Kymeta.Cloud.Services.Toolbox.Extensions;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.SalesOrder.Activities;

/// <summary>
/// Input: SalesforceNeoApproveOrderModel
/// Output: SalesOrderModel
/// </summary>
public class Step2_GetSalesOrderLinesActivity : TaskActivity<Event_SalesforceNeoApproveOrderModel, Step2_GetSalesOrderDetailsModel>
{
    private readonly ITransactionLoggingService _transLog;
    private readonly ILogger<Step2_GetSalesOrderLinesActivity> _logger;
    public Step2_GetSalesOrderLinesActivity(ITransactionLoggingService transLog, ILogger<Step2_GetSalesOrderLinesActivity> logger)
    {
        _transLog = transLog.NotNull();
        _logger = logger.NotNull();
    }

    protected override Step2_GetSalesOrderDetailsModel Execute(TaskContext context, Event_SalesforceNeoApproveOrderModel input)
    {
        _transLog.Add(this.GetMethodName(), context.OrchestrationInstance.InstanceId, input);

        var result = new Step2_GetSalesOrderDetailsModel();

        _transLog.Add(this.GetMethodName(), context.OrchestrationInstance.InstanceId, result);
        return result;

    }
}
