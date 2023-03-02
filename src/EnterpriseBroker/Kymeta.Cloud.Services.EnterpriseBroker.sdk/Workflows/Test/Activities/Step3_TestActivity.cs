using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services.TransactionLog;
using Kymeta.Cloud.Services.Toolbox.Extensions;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.SalesOrder.Activities;

/// <summary>
/// Input: SalesOrderModel
/// Output: OracleSalesOrderResponseModel
/// </summary>
public class Step3_TestActivity : AsyncTaskActivity<Step2_TestModel, Step3_TestModel>
{
    private readonly ITransactionLoggingService _transLog;
    private readonly ILogger<Step3_TestActivity> _logger;
    public Step3_TestActivity(ITransactionLoggingService transLog, ILogger<Step3_TestActivity> logger)
    {
        _transLog = transLog;
        _logger = logger;
    }

    protected override Task<Step3_TestModel> ExecuteAsync(TaskContext context, Step2_TestModel input)
    {
        _transLog.Add(this.GetMethodName(), context.OrchestrationInstance.InstanceId, input);

        var result = new Step3_TestModel();

        _transLog.Add(this.GetMethodName(), context.OrchestrationInstance.InstanceId, result);
        return Task.FromResult(result);
    }
}
