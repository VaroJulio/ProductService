using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Infrastructure.Cache;

namespace ProductService.Infrastructure.Test
{
    public class ProductStatusCacheTest
    {
        public ProductStatusCache GetSystemUnderTest()
        {
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            var memoryCache = serviceProvider.GetService<IMemoryCache>();
            return new ProductStatusCache(memoryCache!);
        }

        [Fact]
        public async void Shoul_Init_Cache()
        {
            var sut = GetSystemUnderTest();

            await sut.InitResetCacheAsync();

            Assert.NotNull(sut.productStatus);
        }

        [Fact]
        public async void Shoul_Get_Product_Status_From_Cache()
        {
            var sut = GetSystemUnderTest();

            await sut.InitResetCacheAsync();
            var statusNames = await sut.GetProductStatusFromCacheOrCreateAsync();

            Assert.Multiple(() =>
            {
                Assert.NotNull(statusNames);
                statusNames.Count.Should().Be(2);
                statusNames[0].Should().Be("Inactive");
                statusNames[1].Should().Be("Active");
            });
        }

        public class Test
        {
            private readonly IMemoryCache _memoryCache;

            public Test(IMemoryCache memoryCache)
            {
                _memoryCache = memoryCache;
            }

            public void SetCache(string key, string value)
            {
                _memoryCache.Set(key, value, new MemoryCacheEntryOptions { SlidingExpiration = TimeSpan.FromHours(1) });
            }
        }
    }
}

