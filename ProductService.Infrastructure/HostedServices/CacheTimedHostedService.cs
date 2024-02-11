using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProductService.Infrastructure.Cache;

namespace ProductService.Infrastructure.IHostedServices
{
    internal class CacheTimedHostedService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<CacheTimedHostedService> _logger;
        private Timer? _timer = null;
        private readonly ProductStatusCache productStatusCache;

        public CacheTimedHostedService(ILogger<CacheTimedHostedService> logger, ProductStatusCache productStatusCache)
        {
            _logger = logger;
            this.productStatusCache = productStatusCache;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Cache Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(300));

            return Task.CompletedTask;
        }

        private async void DoWork(object? state)
        {
            var count = Interlocked.Increment(ref executionCount);

            await productStatusCache.InitResetCacheAsync();

            _logger.LogInformation(
                "Cache Timed Hosted Service is working. Count: {Count}", count);
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Cache Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
