using CometD.NetCore.Bayeux;
using CometD.NetCore.Bayeux.Client;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models;
using Kymeta.Cloud.Services.Toolbox.Extensions;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services;

public class MessageChannelListener : IMessageListener
{
    private readonly ILogger<MessageChannelListener> _logger;
    private MessageEventSubscription? _subscription;
    private readonly ReplayIdStoreService _replayIdStore;
    private readonly IMessageRouter _messageRouter;

    public MessageChannelListener(ReplayIdStoreService replayIdStore, IMessageRouter messageRouter, ILogger<MessageChannelListener> logger)
    {
        _replayIdStore = replayIdStore.NotNull();
        _messageRouter = messageRouter.NotNull();
        _logger = logger;
    }

    public MessageChannelListener SetSubscription(MessageEventSubscription subscription) => this.Action(x => _subscription = subscription);

    public long GetReplayId() =>
        GetSubscription()
        .Func(x => x.ReplayId ?? _replayIdStore.GetReplayId(x.Channel));

    public void OnMessage(IClientSessionChannel channel, IMessage message)
    {
        var convertedJson = message.Json;
        var subscription = GetSubscription();

        _logger.LogInformation("Received message from channel={channel}, replayId={replayId}, json={json}", message.Channel, message.ReplayId, convertedJson);

        var msg = new MessageEventContent
        {
            Channel = message.Channel,
            ChannelId = message.ChannelId.ToString(),
            ReplayId = message.ReplayId,
            Json = convertedJson
        };

        _logger.LogInformation("Calling orchestration message from channel={channel}, replayId={replayId}", message.Channel, message.ReplayId);

        Task.Run(async () => await (subscription?.Forward != null ? subscription.Forward(msg) : _messageRouter.RunOrchestration(msg)))
            .GetAwaiter()
            .GetResult();

        if (subscription.ReplayId == null)
        {
            _logger.LogInformation("Set replay id for channel={channel}, replayId={replayId}", message.Channel, message.ReplayId);

            _replayIdStore.SetReplayId(message.Channel, message.ReplayId);
        }
    }

    private MessageEventSubscription GetSubscription() => _subscription.NotNull(message: "Subscription not set");
}
