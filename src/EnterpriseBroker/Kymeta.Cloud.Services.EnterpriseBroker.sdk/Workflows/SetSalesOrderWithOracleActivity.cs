using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows;

/// <summary>
/// Input: OracleSalesOrderResponseModel
/// Output: bool
/// </summary>
public class SetSalesOrderWithOracleActivity : TaskActivity<string, string>
{
    private readonly ILogger<SetSalesOrderWithOracleActivity> _logger;
    public SetSalesOrderWithOracleActivity(ILogger<SetSalesOrderWithOracleActivity> logger) => _logger = logger;

    protected override string Execute(TaskContext context, string input)
    {
        return input + "." + nameof(SetSalesOrderWithOracleActivity);
    }
}