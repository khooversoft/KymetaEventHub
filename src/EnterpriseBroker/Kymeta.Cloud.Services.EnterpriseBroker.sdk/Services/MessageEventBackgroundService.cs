using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Application;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services;

public class MessageEventBackgroundService : BackgroundService
{
    private readonly MessageEventService _salesForceEventListener;
    private readonly ILogger<MessageEventBackgroundService> _logger;
    private readonly ServiceOption _serviceOption;
    private readonly SalesforceClient2 _client;

    public MessageEventBackgroundService(MessageEventService salesForceEventListener, ServiceOption serviceOption, SalesforceClient2 client, ILogger<MessageEventBackgroundService> logger)
    {
        _salesForceEventListener = salesForceEventListener.NotNull();
        _serviceOption = serviceOption.NotNull();
        _logger = logger.NotNull();
        _client = client;
    }

    protected override async Task ExecuteAsync(CancellationToken token)
    {
        _logger.LogInformation($"EventsBackgroundOperationService Background Service is starting.");

        try
        {
            while (!token.IsCancellationRequested)
            {
                await _salesForceEventListener.Run(GetSubscriptions(), token);
            };
        }
        catch (Exception ex) when (token.IsCancellationRequested)
        {
            _logger.LogWarning(ex, "SalesforcePlatformEvents Background Operation execution cancelled.");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "[BACKGROUND] SalesforcePlatformEvents Background Service execution stopping due to an unhandeled exception.");
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Background Operation Service is stopping.");
        await base.StopAsync(stoppingToken);
    }

    private void ProcessMessage(MessageEventContent messageEventContent)
    {
        _logger.LogInformation("ProcessingMessage: message={message}", messageEventContent);


    }

    private IReadOnlyList<MessageEventSubscription> GetSubscriptions() => new MessageEventSubscription[]
    {
        new MessageEventSubscription
        {
            Channel = $"/event/{_serviceOption.Salesforce.PlatformEvents.Channels.Asset}",
            Forward = x => ProcessMessage(x)
        },
    };
}