using Kymeta.Cloud.Services.Toolbox.Tools;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.InvoiceCreate.Model;

public record Event_InvoiceCreateModel
{
    public string NEO_id__c { get; init; } = null!;
    public string NEO_Order_Number__c { get; init; } = null!;
    public string NEO_Invoice_Type__c { get; init; } = null!;    
    public string NEO_Oracle_Fulfillment_Id__c { get; init; } = null!;  // Comma seperated list of values
    public DateTime NEO_Posted_Date__c { get; init; }
    public string NEO_Oracle_Bill_to_Address_ID__c { get; init; } = null!;
    public string NEO_Oracle_Sales_Order_Id__c { get; init; } = null!;
    public string NEO_PO_Number__c { get; init; } = null!;
    public string NEO_Preferred_Contact_Method__c { get; init; } = null!;
    public DateTime NEO_Invoice_Date__c { get; init; }
    public DateTime NEO_Invoice_Due_Date__c { get; init; }
    public string NEO_Business_Unit__c { get; init; } = null!;
    public string NEO_Invoice_Status__c { get; init; } = null!;
    public string NEO_Invoice_Number__c { get; init; } = null!;
    public string NEO_Oracle_Account_ID__c { get; init; } = null!;
    public string NEO_Notes__c { get; init; } = null!;
    public string NEO_Oracle_Primary_Contact_ID__c { get; init; } = null!;
    public string CreatedById { get; init; } = null!;
    public string NEO_Currency__c { get; init; } = null!;
    public DateTime CreatedDate { get; init; }
    public string NEO_Order__c { get; init; } = null!;
    public string NEO_Oracle_Bill_to_Contact_ID__c { get; init; } = null!;
    public string NEO_Bill_To_Name__c { get; init; } = null!;
    public string NEO_Sales_Representative__c { get; init; } = null!;
    public DateTime NEO_InvoiceDate__c { get; init; }
    public DateTime NEO_InvoicePostedDate__c { get; init; }
    public string NEO_Ship_To_Customer_Number__c { get; init; } = null!;
    public string? NEO_Oracle_Ship_to_Address_ID__c { get; init; }
    public string NEO_Payment_Term__c { get; init; } = null!;
}


public static class Event_InvoiceCreateModelExtensions
{
    public static IReadOnlyList<string> GetFulfillmentIds(this Event_InvoiceCreateModel subject) => subject.NEO_Oracle_Fulfillment_Id__c.NotEmpty()
        .Split(",", StringSplitOptions.RemoveEmptyEntries)
        .Select(x => x.Trim())
        .ToArray();
}
