﻿using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients.Oracle;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients.Salesforce;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Invoice;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.InvoiceCreate.Model;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.SalesOrder.Activities;
using Kymeta.Cloud.Services.Toolbox.Extensions;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.InvoiceCreate.Activities;

public class Step2_CreateOtherInvoiceActivity : AsyncTaskActivity<CreateOtherInvoiceRequest, OracleCreateInvoiceResponseModel?>
{
    private readonly ITransactionLoggingService _transLog;
    private readonly ILogger<Step2_GetSalesOrderLinesActivity> _logger;
    private readonly OracleClient _oracleClient;
    private readonly SalesforceClient2 _salesforceClient;

    public Step2_CreateOtherInvoiceActivity(SalesforceClient2 salesforceClient, OracleClient oracleClient, ITransactionLoggingService transLog, ILogger<Step2_GetSalesOrderLinesActivity> logger)
    {
        _salesforceClient = salesforceClient.NotNull();
        _oracleClient = oracleClient.NotNull();
        _transLog = transLog.NotNull();
        _logger = logger.NotNull();
    }

    protected override async Task<OracleCreateInvoiceResponseModel?> ExecuteAsync(TaskContext context, CreateOtherInvoiceRequest input)
    {
        using var ls = _logger.LogEntryExit(message: $"InstanceId={context.OrchestrationInstance.InstanceId}");
        _transLog.Add(this.GetMethodName(), context.OrchestrationInstance.InstanceId, input);

        IReadOnlyList<SalesforceInvoiceLineModel> lines = input.Lines.Where(x => x.blng__TotalAmount__c >= 0).ToArray();
        if (lines.Count == 0)
        {
            _logger.LogInformation("No invoice lines with total amount >= 0 to create invoice");
            return null;
        }

        // Create oracle invoice
        var request = new OracleCreateInvoiceModel
        {
            CustomerTransactionId = input.Event.NEO_id__c,
            TransactionNumber = input.Event.NEO_Invoice_Number__c,
            BillToCustomerNumber = input.Event.NEO_Bill_To_Name__c,
            ShipToCustomerNumber = input.Event.NEO_Ship_To_Customer_Number__c,
            ShipToSite = input.Event.NEO_Oracle_Ship_to_Address_ID__c,
            BillingDate = input.Event.blng__InvoicePostedDate__c,
            PaymentTerms = input.Event.NEO_Payment_Term__c,
            TransactionDate = input.Event.NEO_Invoice_Date__c,
            BusinessUnit = input.Event.NEO_Business_Unit__c,
            InvoiceCurrencCode = input.Event.NEO_Currency__c,
            CrossReference = input.Event.NEO_Order__c,
            DueDate = input.Event.NEO_Invoice_Due_Date__c,
            TransactionSource = "Manual",
            InternalNotes = input.Event.NEO_Notes__c,
            Comments = input.Event.NEO_Notes__c,

            InvoiceLines = lines
                .Select((x, i) => new OracleCreateInvoiceLineModel
                {
                    Description = x.blng__Notes__c,
                    LineNumber = i + 1,
                    SalesOrder = input.Event.NEO_Order__c,
                    Quantity = (float)x.blng__Quantity__c,
                    UnitPrice = x.blng__UnitPrice__c,
                    ItemNumber = x.NEO_Product_Code__c
                }).ToArray(),
        };

        OracleCreateInvoiceResponseModel result = await _oracleClient.Invoice.Create(request);


        // Link Salesforce to Oracle Invoice created
        var updateRequest = new SalesforceUpdateInvoiceRequestModel
        {
            NEO_Integration_Error = "Clear",
            NEO_Integration_Status = "Success",
            OracleInvoiceNumber = result.TransactionNumber,
        };

        await _salesforceClient.Invoice.Update(input.Event.NEO_id__c, updateRequest);

        _transLog.Add(this.GetMethodName(), context.OrchestrationInstance.InstanceId, result);
        return result;
    }
}

public record CreateOtherInvoiceRequest
{
    public Event_InvoiceCreateModel Event { get; init; } = null!;
    public IReadOnlyList<SalesforceInvoiceLineModel> Lines { get; init; } = Array.Empty<SalesforceInvoiceLineModel>();
}