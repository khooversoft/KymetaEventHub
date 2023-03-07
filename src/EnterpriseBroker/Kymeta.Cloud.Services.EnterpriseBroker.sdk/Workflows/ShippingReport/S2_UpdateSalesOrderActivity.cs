using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients.Salesforce;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Salesforce;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Shipping;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services.TransactionLog;
using Kymeta.Cloud.Services.Toolbox.Extensions;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.ShippingReport;

internal class S2_UpdateSalesOrderActivity : AsyncTaskActivity<ReportRequestResponse, bool>
{
    private readonly TimeSpan _timeout = TimeSpan.FromMinutes(5);
    private readonly ITransactionLoggingService _transLog;
    private readonly ILogger<S1_GetReportActivity> _logger;
    private readonly SalesforceClient2 _salesForceClient;

    public S2_UpdateSalesOrderActivity(
        SalesforceClient2 salesForceClient,
        ITransactionLoggingService transLog,
        ILogger<S1_GetReportActivity> logger
        )
    {
        _transLog = transLog.NotNull();
        _salesForceClient = salesForceClient.NotNull();
        _logger = logger.NotNull();
    }

    protected override async Task<bool> ExecuteAsync(TaskContext context, ReportRequestResponse input)
    {
        context.NotNull();
        input.NotNull();
        using var ls = _logger.LogEntryExit(message: $"InstanceId={context.OrchestrationInstance.InstanceId}");
        _transLog.Add(this.GetMethodName(), context.OrchestrationInstance.InstanceId, input);

        var summerize = input.Items
            .GroupBy(x => x.FulfillLineId)
            .Select(x => (FulfillLineId: x.Key, model: new UpdateProductModel
            {
                Actual_Ship_Date__c = x.First().ShippedDateAndTime,
                NEO_Shipped_Quantity__c = x.Sum(y => y.FulfilledQuantity),
                NEO_Oracle_Back_Order_Fulfillment_Id__c = x.First().SplitFromFLineId,
                NEO_Oracle_Tracking_Number__c = x.Select(y => y.SplitFromFLineId).Join(",").Truncate(255),

            }))
            .ToArray();

        _logger.LogInformation("Processing udpate for products={update}", summerize.Select(x => x.ToString()).Join(";"));

        foreach (var item in summerize)
        {
            SalesforceSearchResult<FullmentSearchResult> search = await _salesForceClient.SalesOrder.Search(item.FulfillLineId);
            if (search.Records.Count == 0)
            {
                _logger.LogError("Cannot find backorder fulfillment id={id}", item.FulfillLineId);
                continue;
            }

            await _salesForceClient.SalesOrder.UpdateOrderItem(search.Records.First().Id, item.model);
        }

        return true;
    }
}
