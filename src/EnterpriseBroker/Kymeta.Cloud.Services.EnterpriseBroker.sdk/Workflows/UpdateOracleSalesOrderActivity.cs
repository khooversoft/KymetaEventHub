using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows;

public class UpdateOracleSalesOrderActivity : TaskActivity<SalesOrderModel, OracleSalesOrderResponseModel>
{
    private readonly ILogger<UpdateOracleSalesOrderActivity> _logger;
    public UpdateOracleSalesOrderActivity(ILogger<UpdateOracleSalesOrderActivity> logger) => _logger = logger;

    protected override OracleSalesOrderResponseModel Execute(TaskContext context, SalesOrderModel input)
    {
        throw new NotImplementedException();
    }
}
