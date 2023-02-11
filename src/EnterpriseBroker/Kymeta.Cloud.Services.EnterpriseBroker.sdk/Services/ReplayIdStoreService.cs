using Kymeta.Cloud.Commons.Databases.Redis;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services;

public class ReplayIdStoreService
{
    private const string _SALESFORCE_PLATFORM_EVENTS_REPLAY_KEYS = "EB:PlatformEvents:ReplayIds";

    private readonly IRedisClient _client;
    private readonly ILogger<ReplayIdStoreService> _logger;

    public ReplayIdStoreService(IRedisClient client, ILogger<ReplayIdStoreService> logger)
    {
        _client = client.NotNull();
        _logger = logger.NotNull();
    }

    public long GetReplayId(string channel)
    {
        long replayId = _client.HashGet<string>(_SALESFORCE_PLATFORM_EVENTS_REPLAY_KEYS, channel) switch
        {
            null => -1,
            string v => int.TryParse(v, out int value) switch
            {
                false => -1,
                true => value,
            }
        };

        _logger.LogTrace("Getting replay id={id} for channel={channel}", replayId, channel);
        return replayId;
    }

    public void SetReplayId(string channel, long replayId)
    {
        _logger.LogTrace("Setting replay id={id} for channel={channel}", replayId, channel);

        _client.HashSetField(_SALESFORCE_PLATFORM_EVENTS_REPLAY_KEYS, channel, replayId.ToString());
    }
}
