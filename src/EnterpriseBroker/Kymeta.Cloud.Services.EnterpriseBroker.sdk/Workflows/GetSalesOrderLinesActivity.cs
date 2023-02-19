using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows;

public class GetSalesOrderLinesActivity : TaskActivity<SalesforceNeoApproveOrderModel, SalesOrderModel>
{
    private readonly ILogger<GetSalesOrderLinesActivity> _logger;
    public GetSalesOrderLinesActivity(ILogger<GetSalesOrderLinesActivity> logger) => _logger = logger;

    protected override SalesOrderModel Execute(TaskContext context, SalesforceNeoApproveOrderModel input)
    {
        throw new NotImplementedException();
    }
}
