using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Salesforce;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services.TransactionLog;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.ShippingReport;
using Kymeta.Cloud.Services.Toolbox.Extensions;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.InvoiceCreate;

public class ShippingReportOrchestration : TaskOrchestration<bool, string>
{
    private readonly ITransactionLoggingService _transLog;
    private readonly ILogger<ShippingReportOrchestration> _logger;
    public ShippingReportOrchestration(ITransactionLoggingService transLog, ILogger<ShippingReportOrchestration> logger)
    {
        _transLog = transLog.NotNull();
        _logger = logger.NotNull();
    }

    public override async Task<bool> RunTask(OrchestrationContext context, string input)
    {
        using var ls = _logger.LogEntryExit();
        context.LogDetails(this.GetMethodName(), _logger);

        var firstRetryInterval = TimeSpan.FromSeconds(1);
        var maxNumberOfAttempts = 5;
        var backoffCoefficient = 1.1;

        var options = new RetryOptions(firstRetryInterval, maxNumberOfAttempts)
        {
            BackoffCoefficient = backoffCoefficient,
            Handle = HandleError
        };

        try
        {
            string instanceId = context.OrchestrationInstance.InstanceId;
            _transLog.Add(this.GetMethodName(), instanceId, new TransLogItemBuilder().SetIsReplay(context.IsReplaying).SetSubject("scheduled").Build());

            ReportRequestResponse report = await context.ScheduleWithRetry<ReportRequestResponse>(typeof(S1_GetReportActivity), options, "run");

            bool success = await context.ScheduleWithRetry<bool>(typeof(S2_UpdateSalesOrderActivity), options, report);

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

    private bool HandleError(Exception ex)
    {
        _logger.LogError(ex, "Orchestration failed");
        return true;
    }
}
