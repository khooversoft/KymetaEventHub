using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows;

public class UpdateOracleSalesOrderActivity : AsyncTaskActivity<SalesOrderModel, OracleSalesOrderResponseModel>
{
    private readonly ILogger<UpdateOracleSalesOrderActivity> _logger;
    private readonly IOracleRestClient _oracleRestClient;

    public UpdateOracleSalesOrderActivity(ILogger<UpdateOracleSalesOrderActivity> logger, IOracleRestClient oracleRestClient)
    {
        _logger = logger;
        _oracleRestClient = oracleRestClient;
    }

    protected override async Task<OracleSalesOrderResponseModel> ExecuteAsync(TaskContext context, SalesOrderModel input)
    {
        if (!input.IsValid())
        {
            throw new InvalidOperationException("Please provide valid sales order!");
        }
        OracleResponse<GetOrderResponse> found  = await _oracleRestClient.GetOrder(input.OrderKey, default);
        OracleUpdateOrder oracleOrder = MapToOracleOrder(input, found);
        await _oracleRestClient.UpdateOrder(oracleOrder, default);
        return new OracleSalesOrderResponseModel();
    }

    private OracleUpdateOrder MapToOracleOrder(SalesOrderModel order, OracleResponse<GetOrderResponse> found)
    {
        throw new NotImplementedException();
    }
}
