using System;
using System.Threading.Tasks;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Application;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services;
using Kymeta.Cloud.Services.EnterpriseBroker.UnitTests.Application;
using Kymeta.Cloud.Services.Toolbox.Extensions;
using Xunit;

namespace Kymeta.Cloud.Services.EnterpriseBroker.UnitTests.EventMessage;

public class EventMessageTests
{
    [Fact]
    public async Task GivenSalesOrderMessage_ShouldProcess()
    {
        var orchestration = TestApplication.GetRequiredService<OrchestrationService>();

        var option = TestApplication.GetRequiredService<ServiceOption>();

        var data = CreateNeoApproval(option);
        var message = new MessageEventContent
        {
            Channel = option.Salesforce.PlatformEvents.Channels.NeoApproveOrder,
            ChannelId = "channelId",
            ReplayId = -1,
            Json = data.ToJson(),
        };

        await orchestration.RunOrchestration(message);

        await Task.Delay(TimeSpan.FromSeconds(1500));
    }


    private SalesforceNeoApproveOrderModel CreateNeoApproval(ServiceOption option) => new SalesforceNeoApproveOrderModel
    {
        Channel = option.Salesforce.PlatformEvents.Channels.NeoApproveOrder,
        Data = new SalesforceNeoApproveOrderData
        {
            Schema = "schema_1",
            Payload = new SalesforceNeoApproveOrderPayload
            {
                NEO_Oracle_Bill_to_Address_ID__c = "NEO_Oracle_Bill_to_Address_ID__c",
                NEO_Id__c = "NEO_Id__c",
                NEO_Preferred_Contract_Method__c = "NEO_Preferred_Contract_Method__c",
                NEO_Account_Name__c = "NEO_Account_Name__c",
                NEO_Ship_to_Name__c = "NEO_Ship_to_Name__c",
                NEO_Deleted_Item_Id__c = "NEO_Deleted_Item_Id__c",
                CreatedById = "CreatedById",
                NEO_Internal_Company__c = "NEO_Internal_Company__c",
                NEO_Event_Type__c = "NEO_Event_Type__c",
                NEO_Sales_Representative__c = "NEO_Sales_Representative__c",
                NEO_OrderNumbrer__c = "NEO_OrderNumbrer__c",
                NEO_Oracle_Ship_to_Address_ID__c = "NEO_Oracle_Ship_to_Address_ID__c",
                NEO_Oracle_Integration_Status__c = "NEO_Oracle_Integration_Status__c",
                Neo_Oracle_Sync_Status__c = "Neo_Oracle_Sync_Status__c",
                NEO_PO_Number__c = "NEO_PO_Number__c",
                NEO_Primary_Contract__c = "NEO_Primary_Contract__c",
                NEO_PO_Date__c = "NEO_PO_Date__c",
                NEO_Order_Type_Oracle_Sync__c = "NEO_Order_Type_Oracle_Sync__c",
                NEO_Oracle_SO__c = "NEO_Oracle_SO__c",
                NEO_ApprovalStatus__c = "NEO_ApprovalStatus__c",
                NEO_Oracle_Account_ID__c = "NEO_Oracle_Account_ID__c",
                NEO_Oracle_Primary_Contact_ID__c = "NEO_Oracle_Primary_Contact_ID__c",
                NEO_Order_Status__c = "NEO_Order_Status__c",
                CreatedDate = "CreatedDate",
                NEO_Oracle_Bill_to_Contact_ID__c = "NEO_Oracle_Bill_to_Contact_ID__c",
                NEO_Bill_To_Name__c = "NEO_Bill_To_Name__c",
            },
            Event = new SalesorderEventModel
            {
                EventUuid = Guid.NewGuid().ToString(),
                ReplayId = 1,
                EventApiName = option.Salesforce.PlatformEvents.Channels.NeoApproveOrder,
            },
        },
    };
}
