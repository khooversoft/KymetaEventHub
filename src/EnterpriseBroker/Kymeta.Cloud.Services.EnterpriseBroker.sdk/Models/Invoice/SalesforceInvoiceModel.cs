using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Invoice;

public record SalesforceInvoiceModel
{
    public string Id { get; init; } = null!;

    // Invoice number
    public string Name { get; init; } = null!;
    public string NEO_Bill_To_Account_Number__c { get; init; } = null!;
    public string NEO_Ship_To_Account_Number__c { get; init; } = null!;

    // Ship To Site ???
    public string NEO_Ship_To_Account_Name__c { get; init; } = null!;
    public DateTime blng__InvoicePostedDate__c { get; init; }
    public string NEO_Payment_Terms__c { get; init; } = null!;
    public DateTime blng__InvoiceDate__c { get; init; }
    public string NEO_Oracle_Business_Unit__c { get; init; } = null!;
    public string blng__BaseCurrency__c { get; init; } = null!;
    public string blng__Order__c { get; init; } = null!;
    public DateTime blng__DueDate__c { get; init; }
    public string blng__Notes__c { get; init; } = null!;

    public IReadOnlyList<SalesforceInvoiceLineModel> Lines { get; init; } = Array.Empty<SalesforceInvoiceLineModel>();
}


public record SalesforceInvoiceLineModel
{
    public string Id { get; init; } = null!;
    public string blng__Invoice__c { get; init; } = null!;
    public string blng__Notes__c { get; init; } = null!;
    public decimal blng__Quantity__c { get; init; }
    public decimal blng__UnitPrice__c { get; init; }
    public string NEO_Product_Code__c { get; init; } = null!;
    public decimal blng__TotalAmount__c { get; init; }
}
