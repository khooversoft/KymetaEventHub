using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Invoice;

public record SalesforceUpdateInvoiceRequestModel
{
    public string? NEO_Oracle_Invoice_Number__c { get; init; }
    public string? NEO_Integration_Error__c { get; init; }
    public string? NEO_Integration_Status__c { get; init; }
    public string? NEO_Oracle_Invoice_Id__c { get; init; }
}
