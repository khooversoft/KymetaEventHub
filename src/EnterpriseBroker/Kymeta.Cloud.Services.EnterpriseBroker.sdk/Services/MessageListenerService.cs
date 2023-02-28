using System.Collections.Specialized;
using System.Linq;
using System.Net;
using CometD.NetCore.Bayeux.Client;
using CometD.NetCore.Client;
using CometD.NetCore.Client.Extension;
using CometD.NetCore.Client.Transport;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Application;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients.Salesforce;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services;

public class MessageListenerService : IBackgroundHost
{
    private readonly SalesforceAuthClient _salesforceAuthClient;
    private readonly ILogger<MessageListenerService> _logger;
    private readonly OrchestrationConfiguration _orchestrationConfiguration;
    private readonly IServiceProvider _serviceProvider;
    private CancellationTokenSource? _cancellationTokenSource;

    public MessageListenerService(
        SalesforceAuthClient salesforceAuthClient,
        OrchestrationConfiguration orchestrationConfiguration,
        IServiceProvider serviceProvider,
        ILogger<MessageListenerService> logger
        )
    {
        _salesforceAuthClient = salesforceAuthClient.NotNull();
        _orchestrationConfiguration = orchestrationConfiguration.NotNull();
        _serviceProvider = serviceProvider.NotNull();
        _logger = logger.NotNull();
    }

    public Task StartAsync(CancellationToken token)
    {
        _logger.LogInformation("Starting listener");

        var tcs = new TaskCompletionSource<bool>();
        _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);

        _ = Task.Run(async () =>
        {
            var signal = new ManualResetEventSlim(false);

            SalesforceAuthenticationResponse accessToken = (await _salesforceAuthClient.GetAuthToken(token))
                .NotNull(message: "Failed to get auth token")
                .Assert(x => x.IsValid(), message: $"Response Auth {nameof(SalesforceAuthenticationResponse)} is not valid");

            var subscriptions = _orchestrationConfiguration.ChannelMapToOrchestrations.Keys
                .Select(x => new MessageEventSubscription { Channel = $"/event/{x}" })
                .Append(new MessageEventSubscription { Channel = "/meta/connect", Forward = x => MonitorHealth(signal, x), ReplayId = -1 })
                .ToArray();

            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                BayeuxClient? client = RunListener(accessToken, subscriptions, _cancellationTokenSource.Token);
                if (client == null)
                {
                    tcs.SetResult(false);
                    return;
                }

                signal.Wait(_cancellationTokenSource.Token);

                client.Disconnect();
                client.WaitFor(1000, new[] { BayeuxClient.State.DISCONNECTED });

                await Task.Delay(TimeSpan.FromSeconds(5));
            }

            tcs.SetResult(true);
            return;
        });

        return tcs.Task;
    }

    public Task StopAsync(CancellationToken _)
    {
        _cancellationTokenSource?.Cancel();
        return Task.CompletedTask;
    }

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
            var listener = _serviceProvider.GetRequiredService<MessageChannelListener>().SetSubscription(subscription);

            IClientSessionChannel metaEventChannel = bayeuxClient.GetChannel(subscription.Channel, listener.GetReplayId());
            metaEventChannel.Subscribe(listener);

            _logger.LogInformation("Listening for events from Salesforce on the '{metaEventChannel}' channel...", metaEventChannel);
        }

        _logger.LogInformation("BayeuxClient listener running");
        return bayeuxClient;
    }

    private Task MonitorHealth(ManualResetEventSlim manualSignal, MessageEventContent messageEventContent)
    {
        _logger.LogInformation("MonitorHealth: messageEvenContent={content}", messageEventContent);

        if (messageEventContent.Json == "close")
        {
            _logger.LogWarning("MonitorHealth: received error notification, shutting down listener");
            manualSignal.Set();
        }

        return Task.CompletedTask;
    }
}
