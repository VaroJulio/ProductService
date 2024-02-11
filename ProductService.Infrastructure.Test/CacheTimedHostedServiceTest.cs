using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using ProductService.Infrastructure.HostedServices;
using ProductService.UseCases.Interfaces;

namespace ProductService.Infrastructure.Test
{
    public class CacheTimedHostedServiceTest
    {
        [Fact]
        public async void Should_Run_CacheTimedHostedService()
        {
            Dictionary<int, string>? productStatus = [];
            productStatus!.Add(0, "Inactive");
            productStatus!.Add(1, "Active");

            var cacheServiceMock = new Mock<IProductStatusCache>();
            cacheServiceMock.Setup(x => x.InitResetCacheAsync()).Returns(Task.FromResult(Task.CompletedTask));
            cacheServiceMock.Setup(x => x.GetProductStatusFromCacheOrCreateAsync()).ReturnsAsync(productStatus);

            IServiceCollection services = new ServiceCollection();
            services.AddSingleton(Mock.Of<ILogger<CacheTimedHostedService>>());
            services.AddSingleton(cacheServiceMock.Object);
            services.AddSingleton<IHostedServiceWithExecutionCount, CacheTimedHostedService>();

            var serviceProvider = services.BuildServiceProvider();
            var productStatusCacheService = serviceProvider.GetRequiredService<IProductStatusCache>();
            var hostedService = serviceProvider.GetService<IHostedServiceWithExecutionCount>();

            if (hostedService is not null)
            {
                await hostedService.StartAsync(default);
                await Task.Delay(1000);
                await hostedService.StopAsync(CancellationToken.None);
            }

            if (productStatusCacheService is not null)
                await productStatusCacheService.GetProductStatusFromCacheOrCreateAsync();

            if (hostedService is not null)
            {
               hostedService.GetExecutionCount().Result.Should().Be(1);
            }

            if (productStatusCacheService is not null)
            {
                var mockproductStatusCacheService = Mock.Get(productStatusCacheService);
                mockproductStatusCacheService.Verify(_ => _.InitResetCacheAsync(), Times.AtLeastOnce);
                mockproductStatusCacheService.Verify(_ => _.GetProductStatusFromCacheOrCreateAsync(), Times.AtLeastOnce());
            }
            
            if (hostedService is null || productStatusCacheService is null)
                Assert.Fail("Unable to resolve required dependencies");
        }
    }
}