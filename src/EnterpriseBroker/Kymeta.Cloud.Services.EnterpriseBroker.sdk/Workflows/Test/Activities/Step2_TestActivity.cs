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
public class Step2_TestActivity : AsyncTaskActivity<Event_TestModel, Step2_TestModel>
{
    private readonly ITransactionLoggingService _transLog;
    private readonly ILogger<Step2_GetSalesOrderLinesActivity> _logger;
    public Step2_TestActivity(ITransactionLoggingService transLog, ILogger<Step2_GetSalesOrderLinesActivity> logger)
    {
        _transLog = transLog.NotNull();
        _logger = logger.NotNull();
    }

    protected override Task<Step2_TestModel> ExecuteAsync(TaskContext context, Event_TestModel input)
    {
        _transLog.Add(this.GetMethodName(), context.OrchestrationInstance.InstanceId, input);

        var result = new Step2_TestModel();

        _transLog.Add(this.GetMethodName(), context.OrchestrationInstance.InstanceId, result);
        return Task.FromResult(result);

    }
}
