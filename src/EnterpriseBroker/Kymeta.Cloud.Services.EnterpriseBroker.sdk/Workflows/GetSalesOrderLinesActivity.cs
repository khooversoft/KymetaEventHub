using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows;

/// <summary>
/// Input: SalesforceNeoApproveOrderModel
/// Output: SalesOrderModel
/// </summary>
public class GetSalesOrderLinesActivity : TaskActivity<string, string>
{
    private readonly ILogger<GetSalesOrderLinesActivity> _logger;
    public GetSalesOrderLinesActivity(ILogger<GetSalesOrderLinesActivity> logger) => _logger = logger;

    protected override string Execute(TaskContext context, string input)
    {
        return input + "." + nameof(GetSalesOrderLinesActivity);
    }
}
