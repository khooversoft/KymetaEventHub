using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CometD.NetCore.Bayeux.Client;
using CometD.NetCore.Client.Extension;
using CometD.NetCore.Client.Transport;
using CometD.NetCore.Client;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Application;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services;

public class MessageEventService
{
    private readonly ServiceOption _serviceOption;
    private readonly SalesforceAuthClient _salesforceAuthClient;
    private readonly ILogger<MessageEventService> _logger;
    private readonly ILoggerFactory _loggerFactory;
    private readonly ReplayIdStoreService _replayIdStore;
    private CancellationTokenSource? _cancellationTokenSource;

    public MessageEventService(ServiceOption serviceOption, SalesforceAuthClient salesforceAuthClient, ReplayIdStoreService replayIdStore, ILoggerFactory loggerFactory)
    {
        _serviceOption = serviceOption.NotNull();
        _salesforceAuthClient = salesforceAuthClient.NotNull();
        _loggerFactory = loggerFactory.NotNull();
        _replayIdStore = replayIdStore.NotNull();

        _logger = loggerFactory.CreateLogger<MessageEventService>().NotNull();
    }

    public async Task Run(IEnumerable<MessageEventSubscription> subscriptions, CancellationToken token)
    {
        SalesforceAuthenticationResponse accessToken = (await _salesforceAuthClient.GetAuthToken(token))
            .NotNull(message: "Failed to get auth token")
            .Assert(x => x.IsValid(), message: $"Response Auth {nameof(SalesforceAuthenticationResponse)} is not valid");

        _logger.LogInformation("Starting listener");

        var tcs = new TaskCompletionSource<bool>();
        _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);

        _ = Task.Run(() =>
        {
            var signal = new ManualResetEventSlim(false);

            var sub = (subscriptions ?? Array.Empty<MessageEventSubscription>())
                .Append(new MessageEventSubscription { Channel = "/meta/connect", Forward = x => MonitorHealth(signal, x), ReplayId = -1 })
                .ToArray();

            BayeuxClient? client = RunListener(accessToken, sub, _cancellationTokenSource.Token);
            if (client == null)
            {
                tcs.SetResult(false);
                return;
            }

            signal.Wait(_cancellationTokenSource.Token);

            client.Disconnect();
            client.WaitFor(1000, new List<BayeuxClient.State>() { BayeuxClient.State.DISCONNECTED });

            tcs.SetResult(true);
            return;
        });

        await tcs.Task;
    }

    public void Stop() => _cancellationTokenSource?.Cancel();

    private BayeuxClient? RunListener(SalesforceAuthenticationResponse accessToken, IReadOnlyList<MessageEventSubscription> subscriptions, CancellationToken token)
    {
        _logger.LogInformation("Connecting Bayeux Client to Salesforce Platform Events CometD streaming endpoint.");

        Dictionary<string, object> options = new() { { ClientTransport.TIMEOUT_OPTION, 120000 } };
        NameValueCollection authCollection = new() { { HttpRequestHeader.Authorization.ToString(), $"OAuth {accessToken.AccessToken}" } };

        // define the transport
        var transport = new LongPollingTransport(options, new NameValueCollection { authCollection });
        var serverUri = new Uri(accessToken.InstanceUrl);
        var streamingEndpoint = string.Format($"{serverUri.Scheme}://{serverUri.Host}/cometd/43.0");
        var bayeuxClient = new BayeuxClient(streamingEndpoint, new[] { transport });

        // add replay extension to be able to re-process potential missed events in case of service interruption
        bayeuxClient.AddExtension(new ReplayExtension());
        bayeuxClient.Handshake();
        bayeuxClient.WaitFor(1000, new List<BayeuxClient.State>() { BayeuxClient.State.CONNECTED });

        // verify the connection is truly established
        if (!bayeuxClient.Connected)
        {
            _logger.LogCritical("Bayeux Client failed to connect to Streaming API");
            return null;
        }

        _logger.LogInformation($"Activating subscribers for Salesforce Platform Events.");

        foreach (var subscription in subscriptions)
        {
            var listener = new MessageEventListener(subscription, _replayIdStore, _logger);
            IClientSessionChannel metaEventChannel = bayeuxClient.GetChannel(subscription.Channel, listener.GetReplayId());
            metaEventChannel.Subscribe(listener);

            _logger.LogInformation("Listening for events from Salesforce on the '{metaEventChannel}' channel...", metaEventChannel);
        }

        _logger.LogInformation("BayeuxClient listener running");
        return bayeuxClient;
    }

    private void MonitorHealth(ManualResetEventSlim manualSignal, MessageEventContent messageEventContent)
    {
        _logger.LogInformation("MonitorHealth: messageEvenContent={content}", messageEventContent);

        if (messageEventContent.Json == "close")
        {
            _logger.LogWarning("MonitorHealth: received error notification, shutting down listener");
            manualSignal.Set();
        }
    }
}
