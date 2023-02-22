using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;

public record Event_SalesforceNeoApproveOrderModel
{
    public SalesforceNeoApproveOrderData Data { get; init; } = null!;
    public string Channel { get; init; } = null!;
}

public record SalesforceNeoApproveOrderData
{
    public string? Schema { get; init; }
    public SalesforceNeoApproveOrderPayload Payload { get; init; } = null!;
    public SalesorderEventModel Event { get; init; } = null!;

}

public record SalesforceNeoApproveOrderPayload
{
    public string? NEO_Oracle_Bill_to_Address_ID__c { get; init; }
    public string? NEO_Id__c { get; init; }
    public string? NEO_Preferred_Contract_Method__c { get; init; }
    public string? NEO_Account_Name__c { get; init; }
    public string? NEO_Ship_to_Name__c { get; init; }
    public string? NEO_Deleted_Item_Id__c { get; init; }
    public string? CreatedById { get; init; }
    public string? NEO_Internal_Company__c { get; init; }
    public string? NEO_Event_Type__c { get; init; }
    public string? NEO_Sales_Representative__c { get; init; }
    public string? NEO_OrderNumbrer__c { get; init; }
    public string? NEO_Oracle_Ship_to_Address_ID__c { get; init; }
    public string? NEO_Oracle_Integration_Status__c { get; init; }
    public string? Neo_Oracle_Sync_Status__c { get; init; }
    public string? NEO_PO_Number__c { get; init; }
    public string? NEO_Primary_Contract__c { get; init; }
    public string? NEO_PO_Date__c { get; init; }
    public string? NEO_Order_Type_Oracle_Sync__c { get; init; }
    public string? NEO_Oracle_SO__c { get; init; }
    public string? NEO_ApprovalStatus__c { get; init; }
    public string? NEO_Oracle_Account_ID__c { get; init; }
    public string? NEO_Oracle_Primary_Contact_ID__c { get; init; }
    public string? NEO_Order_Status__c { get; init; }
    public string? CreatedDate { get; init; }
    public string? NEO_Oracle_Bill_to_Contact_ID__c { get; init; }
    public string? NEO_Bill_To_Name__c { get; init; }
}


