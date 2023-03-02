using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Application;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.SalesOrders;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services;
using Kymeta.Cloud.Services.EnterpriseBroker.UnitTests.Application;
using Xunit;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Workflows.InvoiceCreate.Model;
using Kymeta.Cloud.Services.Toolbox.Extensions;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using System.Reflection;
using System.IO;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models.Salesforce;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services.TransactionLog;

namespace Kymeta.Cloud.Services.EnterpriseBroker.UnitTests.EventMessage;

public class InvoiceEventTests
{
    [Fact]
    public async Task GivenInvoiceMessage_ShouldProcess()
    {
        var orchestration = TestApplication.GetRequiredService<OrchestrationService>();
        var transLog = TestApplication.GetRequiredService<ITransactionLoggingService>();
        var transBuffer = TestApplication.GetRequiredService<TransactionLoggerBuffer>();

        var option = TestApplication.GetRequiredService<ServiceOption>();

        var data = CreateEvent(option);
        var message = new MessageEventContent
        {
            Channel = "NEO_Posted_Invoice__e",
            ChannelId = "NEO_Posted_Invoice__e",
            ReplayId = -1,
            Json = data.ToJson(),
        };

        (bool success, string? instanceId) = await orchestration.RunOrchestration(message);
        success.Should().BeTrue();

        transBuffer.GetLogItems()
            .Reverse()
            .Where(x => x.InstanceId == instanceId)
            .Where(x => x.Method == "TestOrchestration.RunTask" && x.SubjectJson == "completed")
            .FirstOrDefault().Should().NotBeNull();

        transBuffer.GetLogItems().Count.Should().Be(10);
    }

    private Event_InvoiceCreateModel CreateEvent(ServiceOption option)
    {
        var model = Assembly.GetAssembly(this.GetType())
            .ReadAssemblyResource<SalesforceResponse<Event_InvoiceCreateModel>>("Kymeta.Cloud.Services.EnterpriseBroker.UnitTests.Data.InvoiceEvent.json");

        return model.Data.Payload;
    }
}
