using Microsoft.Extensions.Caching.Memory;
using ProductService.UseCases.Interfaces;

namespace ProductService.Infrastructure.Cache
{
    public class ProductStatusCache : IProductStatusCache
    {
        private readonly IMemoryCache memoryCache;
        public Dictionary<int, string>? productStatus = [];

        public ProductStatusCache(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public async Task InitResetCacheAsync()
        {
            if (memoryCache.TryGetValue("ProductStatus", out productStatus))
                memoryCache.Remove("ProductStatus");
            else
                productStatus ??= [];

            productStatus!.Add(0, "Inactive");
            productStatus!.Add(1, "Active");

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            memoryCache.Set("ProductStatus", productStatus, cacheEntryOptions);

            await Task.FromResult(Task.CompletedTask);
        }

        public async Task<Dictionary<int, string>?> GetProductStatusFromCacheOrCreateAsync()
        {
            return await memoryCache.GetOrCreateAsync(
                "ProductStatus",
                cacheEntry =>
                {
                    cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(305);
                    return Task.FromResult(productStatus);
                });
        }
    }
}
