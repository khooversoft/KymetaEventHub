﻿using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients;

public interface ISalesforceClient2
{
    ISalesforceEventApi Events { get; }
}

public class SalesforceClient2 : ISalesforceClient2
{
    public SalesforceClient2(HttpClient client, ILoggerFactory loggerFactory)
    {
        client.NotNull();
        loggerFactory.NotNull();

        Events = new SalesforceEventApi(client, loggerFactory.CreateLogger<SalesforceEventApi>());
    }

    public ISalesforceEventApi Events { get; }
}
