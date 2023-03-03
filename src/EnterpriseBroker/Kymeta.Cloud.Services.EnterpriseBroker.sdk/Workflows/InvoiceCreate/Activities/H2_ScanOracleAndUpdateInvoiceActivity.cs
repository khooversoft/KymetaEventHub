using System.Diagnostics;
using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients.Oracle;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients.Salesforce;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Invoice;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services.TransactionLog;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.InvoiceCreate.Model;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.SalesOrder.Activities;
using Kymeta.Cloud.Services.Toolbox.Extensions;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.InvoiceCreate.Activities;

public class H2_ScanOracleAndUpdateInvoiceActivity : AsyncTaskActivity<Event_InvoiceCreateModel, bool>
{
    private readonly TimeSpan _timeout = TimeSpan.FromMinutes(5);
    private readonly ITransactionLoggingService _transLog;
    private readonly ILogger<H2_ScanOracleAndUpdateInvoiceActivity> _logger;
    private readonly OracleClient _oracleClient;
    private readonly SalesforceClient2 _salesforceClient;

    public H2_ScanOracleAndUpdateInvoiceActivity(
        SalesforceClient2 salesforceClient, 
        OracleClient oracleClient, 
        ITransactionLoggingService transLog,
        ILogger<H2_ScanOracleAndUpdateInvoiceActivity> logger
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

        OracleInvoiceHeaderModel? invoiceHeader = await FindOracleInvoice(input.GetFulfillmentIds());
        if (invoiceHeader == null)
        {
            _logger.LogError("Oracle did not create invoice timeout of {seconds} seconds for FullFillmentIds={FullFillmentIds}", _timeout.TotalSeconds, input.GetFulfillmentIds());
            return false;
        }

        var updateRequest = new SalesforceUpdateInvoiceRequestModel
        {
            NEO_Integration_Error__c = invoiceHeader != null ? "Clear" : $"Did not find Oracle invoice for fulfillmentIds={input.GetFulfillmentIds()}",
            NEO_Integration_Status__c = invoiceHeader != null ? "Success" : "Failed",
            NEO_Oracle_Invoice_Number__c = invoiceHeader?.CustomerTransactionId.ToString() ?? "< error >",
        };

        await _salesforceClient.Invoice.Update(input.NEO_id__c, updateRequest);
        _transLog.Add(this.GetMethodName(), context.OrchestrationInstance.InstanceId, updateRequest);
        _logger.LogInformation("Found Oracle did not create invoice timeout of {seconds} seconds for FullFillmentId={FullFillmentId}", _timeout.TotalSeconds, input.NEO_Oracle_Fulfillment_Id__c);

        return true;
    }

    public async Task<OracleInvoiceHeaderModel?> FindOracleInvoice(IReadOnlyList<string> fullfillmentIds)
    {
        using var lc = _logger.LogEntryExit();
        var tokenSource = new CancellationTokenSource(_timeout);
        var sw = Stopwatch.StartNew();

        while (!tokenSource.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));

            _logger.LogTrace("Looking up invoice for by all fullFillmentId={fullFillmentId}", fullfillmentIds.Join(","));
            OracleInvoiceHeaderModel? invoiceHeader = await _oracleClient.Invoice.FindInvoiceByDeliveryName(fullfillmentIds);

            if (invoiceHeader != null)
            {
                _logger.LogInformation(
                    "Found oracle invoice created by integration, CustomerTransactionId={CustomerTransactionId}, duration={duration}",
                    invoiceHeader.CustomerTransactionId,
                    TimeSpan.FromMilliseconds(sw.ElapsedMilliseconds)
                    );

                return invoiceHeader;
            }
        }

        _logger.LogError("Oracle did not create invoice within timeout of {seconds} seconds for FullFillmentId={FullFillmentId}", _timeout.TotalSeconds, fullfillmentIds.Join(","));
        return null;
    }
}
