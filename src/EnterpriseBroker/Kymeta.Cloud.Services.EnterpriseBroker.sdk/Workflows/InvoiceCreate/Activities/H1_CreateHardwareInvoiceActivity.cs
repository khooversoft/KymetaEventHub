using System.Diagnostics;
using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients.Oracle;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients.Salesforce;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Invoice;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services.TransactionLog;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.InvoiceCreate.Model;
using Kymeta.Cloud.Services.Toolbox.Extensions;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.InvoiceCreate.Activities;

public class H1_CreateHardwareInvoiceActivity : AsyncTaskActivity<Event_InvoiceCreateModel, bool>
{
    private readonly TimeSpan _timeout = TimeSpan.FromMinutes(5);
    private readonly ITransactionLoggingService _transLog;
    private readonly ILogger<H1_CreateHardwareInvoiceActivity> _logger;
    private readonly OracleClient _oracleClient;
    private readonly SalesforceClient2 _salesforceClient;

    public H1_CreateHardwareInvoiceActivity(
        SalesforceClient2 salesforceClient, 
        OracleClient oracleClient, 
        ITransactionLoggingService transLog, 
        ILogger<H1_CreateHardwareInvoiceActivity> logger
        )
    {
        _transLog = transLog.NotNull();
        _oracleClient = oracleClient.NotNull();
        _salesforceClient = salesforceClient.NotNull();
        _logger = logger.NotNull();
    }

    protected override async Task<bool> ExecuteAsync(TaskContext context, Event_InvoiceCreateModel input)
    {
        context.NotNull();
        input.NotNull();
        using var ls = _logger.LogEntryExit(message: $"InstanceId={context.OrchestrationInstance.InstanceId}");
        _transLog.Add(this.GetMethodName(), context.OrchestrationInstance.InstanceId, input);

        await RunOracleIntegrationJob(input);
 
        return true;
    }

    private async Task RunOracleIntegrationJob(Event_InvoiceCreateModel input)
    {
        using var lc = _logger.LogEntryExit();

        var list = new[]
{
            "300000001130195",  // May be an environment specific value (configuration)
            "Distributed Order Orchestration",
            input.NEO_Posted_Date__c.ToString("yyyy-MM-dd"),
            "#NULL,#NULL,#NULL,#NULL,#NULL,#NULL,#NULL,#NULL,#NULL,#NULL,#NULL",
            $"{input.NEO_Order_Number__c}",
            $"{input.NEO_Order_Number__c}",
            "#NULL,#NULL,#NULL,#NULL,#NULL,#NULL,Y,#NULL"
        };

        var request = new OrcaleErpIntegrationsRequestModel
        {
            ESSParameters = list.Join(","),
        };

        var response = await _oracleClient.Integration.PostRequest(request);
        _logger.LogInformation("Posting to Oracle integration");
    }
}
