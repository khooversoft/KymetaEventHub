using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients.Salesforce;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services.TransactionLog;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.SalesOrder2.Model;
using Kymeta.Cloud.Services.Toolbox.Extensions;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.SalesOrder2.Activities;

public class S1_GetOrderItemDetails : AsyncTaskActivity<Event_ApprovedOrderModel, SalesforceOrderHeaderModel>
{
    private readonly ITransactionLoggingService _transLog;
    private readonly ILogger<S1_GetOrderItemDetails> _logger;
    private readonly SalesforceClient2 _salesforceClient;

    public S1_GetOrderItemDetails(
        SalesforceClient2 salesforceClient,
        ITransactionLoggingService transLog,
        ILogger<S1_GetOrderItemDetails> logger
        )
    {
        _transLog = transLog.NotNull();
        _salesforceClient = salesforceClient.NotNull();
        _logger = logger.NotNull();
    }

    protected override async Task<SalesforceOrderHeaderModel> ExecuteAsync(TaskContext context, Event_ApprovedOrderModel input)
    {
        context.NotNull();
        input.NotNull();
        using var ls = _logger.LogEntryExit(message: $"InstanceId={context.OrchestrationInstance.InstanceId}");
        _transLog.Add(this.GetMethodName(), context.OrchestrationInstance.InstanceId, input);

        var response = await _salesforceClient.SalesOrder.SearchByOrderIdForSyncToOracle(input.NEO_Id__c);

        _transLog.Add(this.GetMethodName(), context.OrchestrationInstance.InstanceId, response);
        _logger.LogInformation("Posting to Oracle integration");

        return new SalesforceOrderHeaderModel
        {
            Items = response.Records.ToArray(),
        };
    }
}

