using Kymeta.Cloud.Services.Toolbox.Extensions;
using Kymeta.Cloud.Services.Toolbox.Tools;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Services.TransactionLog;

public class TransactionLoggingFileProvider : ITransactionLogger, IDisposable
{
    private readonly string _baseFileName;
    private readonly Deferred<Action<TransactionLogItem>> _deferred;
    private readonly int _limit;
    private readonly string _loggingFileName;
    private readonly string _loggingFolder;
    private StreamWriter _logFile = null!;

    public TransactionLoggingFileProvider(string loggingFolder, string baseLogFileName, int limit = 10)
    {
        loggingFolder.NotEmpty();
        baseLogFileName.NotEmpty();
        limit.Assert(x => x > 0, "Limit must be greater then 0");

        _loggingFolder = loggingFolder;
        _baseFileName = baseLogFileName;
        _limit = limit;

        _loggingFileName = Path.Combine(_loggingFolder, $"{_baseFileName}_{GetTimestampInFormat()}.log");

        _deferred = new Deferred<Action<TransactionLogItem>>(Initialize);
    }

    public void Log(TransactionLogItem transactionLogItem) => _deferred.Value(transactionLogItem);

    public void Dispose() => Interlocked.Exchange(ref _logFile, null!)?.Dispose();

    private static string GetTimestampInFormat() => DateTime.Now.ToString("o").Replace('.', '_').Replace(':', '_');

    private Action<TransactionLogItem> Initialize()
    {
        Directory.CreateDirectory(_loggingFolder);

        Directory.GetFiles(_loggingFolder, $"{_baseFileName}*.log")
            .OrderByDescending(x => x)
            .Skip(_limit)
            .ForEach(x => File.Delete(x));

        _logFile = new StreamWriter(_loggingFileName);
        _logFile.AutoFlush = true;

        return x => _logFile.WriteLine(x);
    }
}