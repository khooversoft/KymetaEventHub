using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services.TransactionLog;
using Kymeta.Cloud.Services.Toolbox.Extensions;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.SalesOrder.Activities;

/// <summary>
/// Input: OracleSalesOrderResponseModel
/// Output: bool
/// </summary>
public class Step4_TestActivity : AsyncTaskActivity<Step3_TestModel, bool>
{
    private readonly ITransactionLoggingService _transLog;
    private readonly ILogger<Step4_TestActivity> _logger;
    public Step4_TestActivity(ITransactionLoggingService transLog, ILogger<Step4_TestActivity> logger)
    {
        _transLog = transLog.NotNull();
        _logger = logger.NotNull();
    }

    protected override Task<bool> ExecuteAsync(TaskContext context, Step3_TestModel input)
    {
        _transLog.Add(this.GetMethodName(), context.OrchestrationInstance.InstanceId, input);

        return Task.FromResult(true);
    }
}