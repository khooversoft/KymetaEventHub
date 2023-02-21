using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models;
using Kymeta.Cloud.Services.Toolbox.Extensions;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services;

public interface IMessageRouter
{
    Task RunOrchestration(MessageEventContent messageEvent);
}

public class MessageRouter : IMessageRouter
{
    private readonly OrchestrationService _orchestrationService;
    private readonly ILogger<MessageRouter> _logger;

    public MessageRouter(OrchestrationService orchestrationService, ILogger<MessageRouter> logger)
    {
        _orchestrationService = orchestrationService.NotNull();
        _logger = logger.NotNull();
    }

    public async Task RunOrchestration(MessageEventContent messageEvent)
    {
        using var ls = _logger.LogEntryExit();

        _logger.LogInformation("Running orchestration for channel={channel}", messageEvent.Channel);
        await _orchestrationService.RunOrchestration(messageEvent);
    }
}
