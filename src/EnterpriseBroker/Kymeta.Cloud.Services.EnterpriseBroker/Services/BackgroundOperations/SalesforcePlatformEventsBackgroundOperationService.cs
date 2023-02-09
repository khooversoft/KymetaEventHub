using Cronos;
using Microsoft.Extensions.Hosting;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Services.BackgroundOperations
{
    public class SalesforcePlatformEventsBackgroundOperationService : BackgroundService
    {
        private readonly ILogger<SalesforcePlatformEventsBackgroundOperationService> _logger;
        public IServiceProvider Services { get; }

        public SalesforcePlatformEventsBackgroundOperationService(IServiceProvider services, ILogger<SalesforcePlatformEventsBackgroundOperationService> logger)
        {
            Services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation($"SalesforcePlatformEvents Background Service is starting.");

                // init the subscribers (listeners)
                await SalesforcePlatformEventsListen(stoppingToken);
            }
            catch (Exception ex) when (stoppingToken.IsCancellationRequested)
            {
                _logger.LogWarning(ex, "SalesforcePlatformEvents Background Operation execution cancelled.");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "[BACKGROUND] SalesforcePlatformEvents Background Service execution stopping due to an unhandeled exception.");
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Background Operation Service is stopping.");
            await base.StopAsync(stoppingToken);
        }

        /// <summary>
        /// Subscribe to events published by Salesforce
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        private async Task SalesforcePlatformEventsListen(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Synchronize SalesforcePlatformEvents sales orders is working!");

            using IServiceScope scope = Services.CreateScope();
            var platformEventsProcessingService = scope.ServiceProvider.GetRequiredService<ISalesforcePlatformEventsProcessingService>();
            await platformEventsProcessingService.PlatformEventsListen(stoppingToken);
        }
    }
}
