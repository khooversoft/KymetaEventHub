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
public class Step3_SetOracleSalesOrderActivity : TaskActivity<Step2_GetSalesOrderDetailsModel, Step3_OracleSalesOrderResponseModel>
{
    private readonly ITransactionLoggingService _transLog;
    private readonly ILogger<Step3_SetOracleSalesOrderActivity> _logger;
    public Step3_SetOracleSalesOrderActivity(ITransactionLoggingService transLog, ILogger<Step3_SetOracleSalesOrderActivity> logger)
    {
        _transLog = transLog;
        _logger = logger;
    }

    protected override Step3_OracleSalesOrderResponseModel Execute(TaskContext context, Step2_GetSalesOrderDetailsModel input)
    {
        _transLog.Add(this.GetMethodName(), context.OrchestrationInstance.InstanceId, input);

        var result = new Step3_OracleSalesOrderResponseModel();

        _transLog.Add(this.GetMethodName(), context.OrchestrationInstance.InstanceId, result);
        return result;
    }
}
