using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients.Oracle;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Salesforce;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services.TransactionLog;
using Kymeta.Cloud.Services.Toolbox.Extensions;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.ShippingReport;

public class S1_GetReportActivity : AsyncTaskActivity<string, ReportRequestResponse>
{
    private readonly TimeSpan _timeout = TimeSpan.FromMinutes(5);
    private readonly ITransactionLoggingService _transLog;
    private readonly ILogger<S1_GetReportActivity> _logger;
    private readonly OracleReportApi _reportClient;

    public S1_GetReportActivity(
        OracleReportApi reportClient,
        ITransactionLoggingService transLog,
        ILogger<S1_GetReportActivity> logger
        )
    {
        _transLog = transLog.NotNull();
        _reportClient = reportClient.NotNull();
        _logger = logger.NotNull();
    }

    protected override async Task<ReportRequestResponse> ExecuteAsync(TaskContext context, string input)
    {
        context.NotNull();
        using var ls = _logger.LogEntryExit(message: $"InstanceId={context.OrchestrationInstance.InstanceId}");
        _transLog.Add(this.GetMethodName(), context.OrchestrationInstance.InstanceId, input);

        DateTime date1 = DateTime.UtcNow.AddDays(-1);
        //DateTime date1 = DateTime.Parse("02-15-2023");

        _logger.LogInformation("Getting report for shipped from Oracle");
        ReportRequestResponse report = await _reportClient.Get(date1);

        return report;
    }
}
