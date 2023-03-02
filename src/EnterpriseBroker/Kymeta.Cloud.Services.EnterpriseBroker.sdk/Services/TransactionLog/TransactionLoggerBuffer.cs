using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kymeta.Cloud.Services.Toolbox.Extensions;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services.TransactionLog;

public class TransactionLoggerBuffer : ITransactionLogger
{
    private readonly ConcurrentQueue<TransactionLogItem> _queue = new ConcurrentQueue<TransactionLogItem>();
    private const int _max = 1000;

    public void Log(TransactionLogItem item) => _queue
        .Action(x =>
        {
            while (x.Count > _max)
            {
                x.TryDequeue(out var _);
            }

            x.Enqueue(item);
        });

    public IReadOnlyList<TransactionLogItem> GetLogItems(long? replayId = null) => replayId switch
    {
        long v => _queue.Where(x => x.ReplayId >= v).ToArray(),
        _ => _queue.ToArray(),
    };
}
