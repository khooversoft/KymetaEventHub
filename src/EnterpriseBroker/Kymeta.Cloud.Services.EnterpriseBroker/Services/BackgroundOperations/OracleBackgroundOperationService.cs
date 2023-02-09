using Microsoft.Extensions.Hosting;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Services.BackgroundOperations
{
    public class OracleBackgroundOperationService : BackgroundService
    {
        private readonly ILogger<OracleBackgroundOperationService> _logger;
        public IServiceProvider Services { get; }

        public OracleBackgroundOperationService(IServiceProvider services, ILogger<OracleBackgroundOperationService> logger)
        {
            Services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation($"Oracle Background Service is starting.");

                // init the sync from Oracle to SerialCache DB for Sales Orders
                await SynchronizeOracleSalesOrders(stoppingToken);
            }
            catch (Exception ex) when (stoppingToken.IsCancellationRequested)
            {
                _logger.LogWarning(ex, "Oracle Background Operation execution cancelled.");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Oracle Background Service execution stopping due to an unhandeled exception.");
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Background Operation Service is stopping.");
            await base.StopAsync(stoppingToken);
        }

        /// <summary>
        /// Synchronize "Sales Order" objects from Oracle to our Cloud SerialCache database
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        private async Task SynchronizeOracleSalesOrders(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Synchronize Oracle sales orders is working!");

            using IServiceScope scope = Services.CreateScope();
            var oracleProcessingService = scope.ServiceProvider.GetRequiredService<IOracleProcessingService>();
            await oracleProcessingService.SynchronizeSalesOrders(stoppingToken);
        }
    }
}
