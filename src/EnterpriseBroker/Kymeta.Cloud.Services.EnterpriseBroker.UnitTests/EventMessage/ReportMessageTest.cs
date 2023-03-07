using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Application;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services.TransactionLog;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services;
using Kymeta.Cloud.Services.EnterpriseBroker.UnitTests.Application;
using Xunit;

namespace Kymeta.Cloud.Services.EnterpriseBroker.UnitTests.EventMessage;

public class ReportMessageTest
{
    [Fact]
    public async Task GivenInvoiceMessage_ShouldProcess()
    {
        var orchestration = TestApplication.GetRequiredService<OrchestrationService>();
        var transLog = TestApplication.GetRequiredService<ITransactionLoggingService>();
        var transBuffer = TestApplication.GetRequiredService<TransactionLoggerBuffer>();

        var option = TestApplication.GetRequiredService<ServiceOption>();

        var message = new MessageEventContent
        {
            Channel = "oracleReport",
            ChannelId = "oracleReport",
            ReplayId = -1,
            Json = string.Empty,
        };

        (bool success, string? instanceId) = await orchestration.RunOrchestration(message);
        success.Should().BeTrue();

        transBuffer.GetLogItems()
            .Reverse()
            .Where(x => x.InstanceId == instanceId)
            .Where(x => x.Method == "ShippingReportOrchestration.RunTask" && x.SubjectJson == "completed")
            .FirstOrDefault().Should().NotBeNull();
    }
}
