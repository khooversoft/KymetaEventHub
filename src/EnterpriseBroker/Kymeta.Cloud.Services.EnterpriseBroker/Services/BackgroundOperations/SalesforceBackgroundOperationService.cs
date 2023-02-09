using Microsoft.Extensions.Hosting;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Services.BackgroundOperations
{
    public class SalesforceBackgroundOperationService : BackgroundService
    {
        private readonly ILogger<SalesforceBackgroundOperationService> _logger;
        public IServiceProvider Services { get; }

        public SalesforceBackgroundOperationService(IServiceProvider services, ILogger<SalesforceBackgroundOperationService> logger)
        {
            Services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation($"Salesforce Background Operation Service is starting.");
                // init the sync from Salesforce to Kymeta Cloud (OSS)
                await SynchronizeSalesforceProducts(stoppingToken);
            }
            catch (Exception ex) when (stoppingToken.IsCancellationRequested)
            {
                _logger.LogWarning(ex, "Salesforce Background Operation execution cancelled.");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Salesforce Background Service execution stopping due to an unhandeled exception.");
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Salesforce Background Operation Service is stopping.");
            await base.StopAsync(stoppingToken);
        }

        /// <summary>
        /// Synchronize "Product" objects from Salesforce to our Cloud Redis cache
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        private async Task SynchronizeSalesforceProducts(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Synchronize Salesforce Products is working!");

            using IServiceScope scope = Services.CreateScope();
            var sfProcessingService = scope.ServiceProvider.GetRequiredService<ISalesforceProcessingService>();
            await sfProcessingService.SynchronizeProducts(stoppingToken);
        }
    }
}
