using CometD.NetCore.Bayeux;
using CometD.NetCore.Bayeux.Client;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services;

public class MessageEventListener : IMessageListener
{
    private readonly ILogger _logger;
    private readonly MessageEventSubscription _subscription;
    private readonly ReplayIdStoreService _replayIdStore;

    public MessageEventListener(MessageEventSubscription subscription, ReplayIdStoreService replayIdStore, ILogger logger)
    {
        _replayIdStore = replayIdStore.NotNull();
        _subscription = subscription.NotNull();
        _logger = logger.NotNull();
    }

    public long GetReplayId() => _subscription.ReplayId ?? _replayIdStore.GetReplayId(_subscription.Channel);

    public void OnMessage(IClientSessionChannel channel, IMessage message)
    {
        var convertedJson = message.Json;

        _logger.LogInformation("[EB] Received message from channel, replayId={replayId}, json={json}", message.ReplayId, convertedJson);

        _subscription.Forward(new MessageEventContent
        {
            Channel = message.Channel,
            ChannelId = message.ChannelId.ToString(),
            ReplayId = message.ReplayId,
            Json = convertedJson
        });

        if (_subscription.ReplayId == null)
        {
            _replayIdStore?.SetReplayId(message.Channel, message.ReplayId);
        }
    }
}
