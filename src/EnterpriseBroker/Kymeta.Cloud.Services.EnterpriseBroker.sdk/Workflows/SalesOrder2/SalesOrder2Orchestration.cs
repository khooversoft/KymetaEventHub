using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services.TransactionLog;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.SalesOrder2.Activities;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.SalesOrder2.Model;
using Kymeta.Cloud.Services.Toolbox.Extensions;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.InvoiceCreate;

public class SalesOrder2Orchestration : TaskOrchestration<bool, string>
{
    private readonly ITransactionLoggingService _transLog;
    private readonly ILogger<SalesOrder2Orchestration> _logger;
    public SalesOrder2Orchestration(ITransactionLoggingService transLog, ILogger<SalesOrder2Orchestration> logger)
    {
        _transLog = transLog.NotNull();
        _logger = logger.NotNull();
    }

    public override async Task<bool> RunTask(OrchestrationContext context, string input)
    {
        using var ls = _logger.LogEntryExit();
        context.LogDetails(this.GetMethodName(), _logger);

        RetryOptions options = WorkflowTools.GetRetryOptions(_logger);

        try
        {
            string instanceId = context.OrchestrationInstance.InstanceId;
            Event_ApprovedOrderModel eventData = input.ToObject<Event_ApprovedOrderModel>().NotNull();
            _transLog.Add(this.GetMethodName(), instanceId, new TransLogItemBuilder().SetIsReplay(context.IsReplaying).SetSubject(eventData).Build());

            SalesforceOrderHeaderModel orderItems =
                await context.ScheduleWithRetry<SalesforceOrderHeaderModel>(typeof(S1_GetOrderItemDetails), options, eventData);




            _transLog.Add(this.GetMethodName(), instanceId, "completed");
            _logger.LogInformation("Completed orchestration");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Orchestration failed");
            return false;
        }
    }
}



