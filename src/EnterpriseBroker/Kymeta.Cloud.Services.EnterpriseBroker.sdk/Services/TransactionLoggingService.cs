using System.Collections.Concurrent;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models;
using Kymeta.Cloud.Services.Toolbox.Extensions;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services;

public interface ITransactionLoggingService
{
    void Add<T>(string method, string instanceId, T subject);
    IReadOnlyList<TransactionLogItem> GetLogItems(long? replayId = null);
}

public class TransactionLoggingService : ITransactionLoggingService
{
    private readonly ILogger<TransactionLoggingService> _logger;
    private readonly History _history = new History();
    private long _replayId = 0;

    public TransactionLoggingService(ILogger<TransactionLoggingService> logger)
    {
        _logger = logger.NotNull();
    }

    public void Add<T>(string method, string instanceId, T subject) => _history.Enqueue(new TransactionLogItem
    {
        InstanceId = instanceId.NotNull(),
        Method = method.NotEmpty(),
        ReplayId = Interlocked.Increment(ref _replayId),
        TypeName = subject.GetTypeName(),
        SubjectJson = subject switch
        {
            string v => v.NotEmpty(),
            _ => subject.NotNull().ToJson(),
        }
    });

    public IReadOnlyList<TransactionLogItem> GetLogItems(long? replayId = null) => _history.GetLogItems(replayId);

    private class History
    {
        private readonly ConcurrentQueue<TransactionLogItem> _queue = new ConcurrentQueue<TransactionLogItem>();
        private const int _max = 1000;

        public void Enqueue(TransactionLogItem item) => _queue
            .Action(x =>
            {
                while (x.Count > _max)
                {
                    x.TryDequeue(out var _);
                }

                x.Enqueue(item);
            });

        public bool TryDequeue(out TransactionLogItem? item) => _queue.TryDequeue(out item);

        public IReadOnlyList<TransactionLogItem> GetLogItems(long? replayId = null) => replayId switch
        {
            long v => _queue.Where(x => x.ReplayId >= v).ToArray(),
            _ => _queue.ToArray(),
        };
    }
}
