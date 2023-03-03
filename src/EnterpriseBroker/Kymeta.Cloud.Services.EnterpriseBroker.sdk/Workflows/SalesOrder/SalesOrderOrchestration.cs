//using DurableTask.Core;
//using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
//using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services.TransactionLog;
//using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.SalesOrder.Activities;
//using Kymeta.Cloud.Services.Toolbox.Extensions;
//using Kymeta.Cloud.Services.Toolbox.Tools;
//using Microsoft.Extensions.Logging;

//namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.SalesOrder;

//public partial class SalesOrderOrchestration : TaskOrchestration<bool, string>
//{
//    private readonly ITransactionLoggingService _transLog;
//    private readonly ILogger<SalesOrderOrchestration> _logger;
//    public SalesOrderOrchestration(ITransactionLoggingService transLog, ILogger<SalesOrderOrchestration> logger)
//    {
//        _transLog = transLog.NotNull();
//        _logger = logger.NotNull();
//    }

//    public override async Task<bool> RunTask(OrchestrationContext context, string input)
//    {
//        using var ls = _logger.LogEntryExit();
//        context.LogDetails(this.GetMethodName(), _logger);

//        var firstRetryInterval = TimeSpan.FromSeconds(1);
//        var maxNumberOfAttempts = 5;
//        var backoffCoefficient = 1.1;

//        var options = new RetryOptions(firstRetryInterval, maxNumberOfAttempts)
//        {
//            BackoffCoefficient = backoffCoefficient,
//            Handle = HandleError
//        };

//        try
//        {
//            string instanceId = context.OrchestrationInstance.InstanceId;
//            Event_SalesforceNeoApproveOrderModel eventData = input.ToObject<Event_SalesforceNeoApproveOrderModel>().NotNull();
//            _transLog.Add(this.GetMethodName(), instanceId, new TransLogItemBuilder().SetIsReplay(context.IsReplaying).SetSubject(eventData).Build());

//            Step2_GetSalesOrderDetailsModel salesOrderModel = await context.ScheduleWithRetry<Step2_GetSalesOrderDetailsModel>(typeof(Step2_GetSalesOrderLinesActivity), options, eventData);

//            Step3_OracleSalesOrderResponseModel oracleResponse = await context.ScheduleWithRetry<Step3_OracleSalesOrderResponseModel>(typeof(Step3_SetOracleSalesOrderActivity), options, salesOrderModel);

//            string success = await context.ScheduleWithRetry<string>(typeof(Step4_UpdateSalesforceSalesOrderActivity), options, oracleResponse);

//            _transLog.Add(this.GetMethodName(), instanceId, "completed");
//            _logger.LogInformation("Completed orchestration");
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Orchestration failed");
//            return false;
//        }

//        return true;
//    }

//    private bool HandleError(Exception ex)
//    {
//        _logger.LogError(ex, "Orchestration failed");
//        return true;
//    }
//}

