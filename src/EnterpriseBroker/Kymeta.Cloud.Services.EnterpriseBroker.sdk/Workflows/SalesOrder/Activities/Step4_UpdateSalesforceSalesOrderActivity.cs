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
public class Step4_UpdateSalesforceSalesOrderActivity : TaskActivity<Step3_OracleSalesOrderResponseModel, bool>
{
    private readonly ITransactionLoggingService _transLog;
    private readonly ILogger<Step4_UpdateSalesforceSalesOrderActivity> _logger;
    public Step4_UpdateSalesforceSalesOrderActivity(ITransactionLoggingService transLog, ILogger<Step4_UpdateSalesforceSalesOrderActivity> logger)
    {
        _transLog = transLog.NotNull();
        _logger = logger.NotNull();
    }

    protected override bool Execute(TaskContext context, Step3_OracleSalesOrderResponseModel input)
    {
        _transLog.Add(this.GetMethodName(), context.OrchestrationInstance.InstanceId, input);

        return true;
    }
}