using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows;

public class SetSalesOrderWithOracleActivity : TaskActivity<OracleSalesOrderResponseModel, bool>
{
    private readonly ILogger<SetSalesOrderWithOracleActivity> _logger;
    public SetSalesOrderWithOracleActivity(ILogger<SetSalesOrderWithOracleActivity> logger) => _logger = logger;

    protected override bool Execute(TaskContext context, OracleSalesOrderResponseModel input)
    {
        throw new NotImplementedException();
    }
}