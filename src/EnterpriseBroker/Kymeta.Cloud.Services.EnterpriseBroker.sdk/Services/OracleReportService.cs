using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DurableTask.Core;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Application;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients.Oracle;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models;
using Kymeta.Cloud.Services.Toolbox.Extensions;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services;

public class OracleReportService : IBackgroundHost
{
    private readonly ILogger<OracleReportService> _logger;
    private readonly IMessageRouter _messageRouter;
    private CancellationTokenSource? _cancellationTokenSource;

    public OracleReportService(IMessageRouter messageRouter, ILogger<OracleReportService> logger)
    {
        _messageRouter = messageRouter.NotNull();
        _logger = logger.NotNull();
    }

    public Task StartAsync(CancellationToken token)
    {
        _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);

        _ = Task.Run(async () =>
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                var message = new MessageEventContent
                {
                    Channel = "oracleReport",
                    ChannelId = "oracleReport",
                    ReplayId = -1,
                    Json = string.Empty,
                };

                _logger.LogInformation("Running orchestration for report");
                await _messageRouter.RunOrchestration(message);
            }
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cancellationTokenSource?.Cancel();
        return Task.CompletedTask;
    }
}
