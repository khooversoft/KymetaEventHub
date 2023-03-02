using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Application;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services.TransactionLog;
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
        var transLog = TestApplication.GetRequiredService<ITransactionLoggingService>();

        await Task.Delay(TimeSpan.FromMinutes(1));
        var option = TestApplication.GetRequiredService<ServiceOption>();

        var data = CreateEvent(option);
        var message = new MessageEventContent
        {
            Channel = "testChannel",
            ChannelId = "channelId",
            ReplayId = -1,
            Json = data.ToJson(),
        };

        (bool success, string? instanceId) = await orchestration.RunOrchestration(message);
        success.Should().BeTrue();

        //transLog.GetLogItems()
        //    .Reverse()
        //    .Where(x => x.InstanceId == instanceId)
        //    .Where(x => x.Method == "TestOrchestration.RunTask" && x.SubjectJson == "completed")
        //    .FirstOrDefault().Should().NotBeNull();

        //transLog.GetLogItems().Count.Should().Be(10);
    }

    private Event_TestModel CreateEvent(ServiceOption option) => new Event_TestModel
    {
        NEO_Oracle_Bill_to_Address_ID__c = "NEO_Oracle_Bill_to_Address_ID__c",
        NEO_Id__c = "NEO_Id__c",
        NEO_Preferred_Contract_Method__c = "NEO_Preferred_Contract_Method__c",
        NEO_Account_Name__c = "NEO_Account_Name__c",
        NEO_Ship_to_Name__c = "NEO_Ship_to_Name__c",
        Channel = "testChannel",
    };
}
