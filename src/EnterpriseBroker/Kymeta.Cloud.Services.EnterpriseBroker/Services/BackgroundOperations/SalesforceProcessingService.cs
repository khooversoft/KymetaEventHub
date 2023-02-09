using Cronos;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Services.BackgroundOperations
{
    internal interface ISalesforceProcessingService
    {
        Task SynchronizeProducts(CancellationToken stoppingToken);
    }

    public class SalesforceProcessingService : ISalesforceProcessingService
    {
        private readonly IConfiguration _config;
        private int executionCount = 0;
        private readonly ILogger _logger;
        private readonly IProductsBrokerService _productsBrokerService;

        public SalesforceProcessingService(IConfiguration config, ILogger<SalesforceProcessingService> logger, IProductsBrokerService productsBrokerService)
        {
            _config = config;
            _logger = logger;
            _productsBrokerService = productsBrokerService;
        }

        public async Task SynchronizeProducts(CancellationToken stoppingToken)
        {
            // prevent execution if background service is stopping
            while (!stoppingToken.IsCancellationRequested)
            {
                // increment count
                executionCount++;
                _logger.LogInformation("Synchronize Products is working. Count: {Count}", executionCount);
                // init the synchronization
                await _productsBrokerService.SynchronizeProducts();
                // fetch synchronize interval from config
                var synchronizeProductsInterval = _config["Intervals:SyncProducts"];
                // error if no config value is present
                if (string.IsNullOrEmpty(synchronizeProductsInterval)) throw new Exception("Missing 'Intervals:SyncProducts' configuration value.");
                // schedule the operation to run on the schedule the cron expression dictates (from Grapevine config)
                await WaitForNextSchedule(synchronizeProductsInterval, stoppingToken);
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
            _logger.LogInformation($"The Synchronize Products worker is delayed for {delay}. Current time: {DateTimeOffset.Now}");
            await Task.Delay(delay, stoppingToken);
        }
    }
}
