using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows;

/// <summary>
/// Input: SalesOrderModel
/// Output: OracleSalesOrderResponseModel
/// </summary>
public class UpdateOracleSalesOrderActivity : TaskActivity<string, string>
{
    private readonly ILogger<UpdateOracleSalesOrderActivity> _logger;
    public UpdateOracleSalesOrderActivity(ILogger<UpdateOracleSalesOrderActivity> logger) => _logger = logger;

    protected override string Execute(TaskContext context, string input)
    {
        return input + "." + nameof(UpdateOracleSalesOrderActivity);
    }
}
