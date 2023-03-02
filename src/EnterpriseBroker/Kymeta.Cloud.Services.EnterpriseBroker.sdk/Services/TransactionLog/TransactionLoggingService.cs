using System.Collections.Concurrent;
using System.Threading.Tasks.Dataflow;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models;
using Kymeta.Cloud.Services.Toolbox.Extensions;
using Kymeta.Cloud.Services.Toolbox.Tools;
using Microsoft.Extensions.Logging;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services.TransactionLog;

public interface ITransactionLoggingService
{
    void Add<T>(string method, string instanceId, T subject);
    void AddProvider(ITransactionLogger provider);
}

public interface ITransactionLogger
{
    void Log(TransactionLogItem transactionLogItem);
}

public class TransactionLoggingService : ITransactionLoggingService, ITransactionLogger
{
    private readonly ConcurrentQueue<ITransactionLogger> _providers = new();
    private long _replayId = 0;

    public void Add<T>(string method, string instanceId, T subject) => Log(new TransactionLogItem
    {
        InstanceId = instanceId.NotNull(),
        Method = method.NotEmpty(),
        ReplayId = Interlocked.Increment(ref _replayId),
        TypeName = subject.GetTypeName(),
        SubjectJson = subject switch
        {
            string v => v.NotEmpty(),
            _ => subject?.ToJson() ?? "<empty>",
        }
    });

    public void AddProvider(ITransactionLogger provider) => this.Action(x => _providers.Enqueue(provider));

    public void Log(TransactionLogItem item) => _providers.ForEach(x => x.Log(item));
}

