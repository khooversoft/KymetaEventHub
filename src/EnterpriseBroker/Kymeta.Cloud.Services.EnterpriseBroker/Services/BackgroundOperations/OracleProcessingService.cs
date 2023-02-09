using Cronos;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Services.BackgroundOperations
{
    internal interface IOracleProcessingService
    {
        Task SynchronizeSalesOrders(CancellationToken stoppingToken);
    }

    public class OracleProcessingService : IOracleProcessingService
    {
        private readonly IConfiguration _config;
        private int executionCount = 0;
        private readonly ILogger _logger;
        private readonly IOracleService _oracleService;

        public OracleProcessingService(IConfiguration config, ILogger<OracleProcessingService> logger, IOracleService oracleService)
        {
            _config = config;
            _logger = logger;
            _oracleService = oracleService;
        }

        public async Task SynchronizeSalesOrders(CancellationToken stoppingToken)
        {
            // prevent execution if background service is stopping
            while (!stoppingToken.IsCancellationRequested)
            {
                // increment count
                executionCount++;
                _logger.LogInformation($"[{DateTimeOffset.Now}] Synchronize Sales Orders is working. Count: {executionCount}");
                // init the synchronization
                var syncResult = await _oracleService.SynchronizeSalesOrders();
                if (!syncResult.Item1) _logger.LogCritical($"[SalesOrderFilling] Failed to synchronize Sales Orders with Oracle: {syncResult.Item2}");
                // fetch synchronize interval from config
                var synchronizeSalesOrdersInterval = _config["Intervals:SalesOrders"];
                // error if no config value is present
                if (string.IsNullOrEmpty(synchronizeSalesOrdersInterval)) throw new Exception("Missing 'Intervals:SalesOrders' configuration value.");
                // schedule the operation to run on the schedule the cron expression dictates (from Grapevine config)
                await WaitForNextSchedule(synchronizeSalesOrdersInterval, stoppingToken);
            }
        }

        private async Task WaitForNextSchedule(string cronExpression, CancellationToken stoppingToken)
        {
            // parse the CRON expression
            var parsedExp = CronExpression.Parse(cronExpression);
            var currentUtcTime = DateTimeOffset.UtcNow.UtcDateTime;
            // calculate the next occurence 
            var occurenceTime = parsedExp.GetNextOccurrence(currentUtcTime).GetValueOrDefault();
            // calculate the delay
            var delay = occurenceTime - currentUtcTime;
            _logger.LogInformation($"The Sales Order synchronization worker is delayed for {delay}. Current time: {DateTimeOffset.Now}");
            await Task.Delay(delay, stoppingToken);
        }
    }
}
