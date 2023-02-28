using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Invoice;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.InvoiceCreate.Activities;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.InvoiceCreate.Model;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.SalesOrder;
using Kymeta.Cloud.Services.Toolbox.Extensions;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.InvoiceCreate;

public class InvoiceCreateOrchestration : TaskOrchestration<bool, string>
{
    private readonly ITransactionLoggingService _transLog;
    private readonly ILogger<TestOrchestration> _logger;
    public InvoiceCreateOrchestration(ITransactionLoggingService transLog, ILogger<TestOrchestration> logger)
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
            Event_InvoiceCreateModel eventData = input.ToObject<Event_InvoiceCreateModel>().NotNull();
            _transLog.Add(this.GetMethodName(), instanceId, new TransLogItemBuilder().SetIsReplay(context.IsReplaying).SetSubject(eventData).Build());

            switch (eventData.NEO_Invoice_Type__c.ToLower())
            {
                case "hardware":
                    await context.ScheduleWithRetry<bool>(typeof(H1_CreateHardwareInvoiceActivity), options, eventData);
                    await context.ScheduleWithRetry<bool>(typeof(H2_ScanOracleAndUpdateInvoiceActivity), options, eventData);
                    break;

                case "other":
                    IReadOnlyList<SalesforceInvoiceLineModel> lineItems = await context.ScheduleWithRetry<IReadOnlyList<SalesforceInvoiceLineModel>>(typeof(Step1_GetInvoiceLineItemsActivity), options, eventData);

                    var otherRequest = new CreateOtherInvoiceRequest
                    {
                        Event = eventData,
                        Lines = lineItems,
                    };

                    OracleCreateInvoiceResponseModel? created = await context.ScheduleWithRetry<OracleCreateInvoiceResponseModel?>(typeof(Step2_CreateOtherInvoiceActivity), options, otherRequest);
                    break;

                default:
                    _logger.LogInformation("Skipping invoiceId={invoiceId}, not supported invoice type={invoiceType}", eventData.NEO_id__c, eventData.NEO_Invoice_Type__c);
                    break;
            }

            _transLog.Add(this.GetMethodName(), instanceId, "completed");
            _logger.LogInformation("Completed orchestration");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Orchestration failed");
            return false;
        }

        return true;
    }

    private bool HandleError(Exception ex)
    {
        _logger.LogError(ex, "Orchestration failed");
        return true;
    }
}
