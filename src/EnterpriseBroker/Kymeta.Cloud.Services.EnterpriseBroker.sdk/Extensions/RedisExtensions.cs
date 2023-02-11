//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Kymeta.Cloud.Commons.Databases.Redis;
//using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Application;
//using Kymeta.Cloud.Services.Toolbox.Tools;
//using StackExchange.Redis;

//namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Extensions;

//public static class RedisExtensions
//{
//    //private const string _tokenText = "EB:SFToken";
//    //private const string _urlText = "EB:SFApiRoot";
//    private const string _SALESFORCE_PLATFORM_EVENTS_REPLAY_KEYS = "EB:PlatformEvents:ReplayIds";

//    //public static (string? token, string? url) GetSalesforceAuthValues(this IRedisClient subject)
//    //{
//    //    subject.NotNull();

//    //    var token = subject.StringGet<string?>(_tokenText).NotEmpty();
//    //    var url = subject.StringGet<string?>(_urlText).NotEmpty();

//    //    return (token, url);
//    //}

//    //public static void SetSalesforceAuthValues(this IRedisClient subject, string? accessToken, string? url, TimeSpan? lifeTime = null)
//    //{
//    //    subject.NotNull();

//    //    lifeTime = lifeTime ?? TimeSpan.FromHours(1);

//    //    subject.StringSet("EB:SFToken", accessToken, TimeSpan.FromHours(1));
//    //    subject.StringSet("EB:SFApiRoot", url, TimeSpan.FromHours(1));
//    //}

//    public static void SetReplayId(this IRedisClient client, string channel, long replayId)
//    {
//        client.NotNull();
//        client.HashSetField(_SALESFORCE_PLATFORM_EVENTS_REPLAY_KEYS, channel, replayId.ToString());
//    }

//    /// <summary>
//    /// Fetch cached ReplayId value from Redis cache
//    /// replayId -1: (Default if no replay option is specified.) Subscriber receives new events that are broadcast after the client subscribes.
//    /// replayId -2: Subscriber receives all events, including past events that are within the retention window and new events.
//    /// </summary>
//    /// <param name="channel"></param>
//    /// <returns></returns>
//    public static int GetReplayId(this IRedisClient client, string channel)
//    {
//        client.NotNull();

//        return client.HashGet<string>(_SALESFORCE_PLATFORM_EVENTS_REPLAY_KEYS, channel) switch
//        {
//            null => -1,
//            string v => int.TryParse(v, out int replayId) switch
//            {
//                false => -1,
//                true => replayId,
//            }
//        };
//    }
//}
