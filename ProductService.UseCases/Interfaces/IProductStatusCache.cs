using Microsoft.Extensions.Caching.Memory;

namespace ProductService.UseCases.Interfaces
{
    public interface IProductStatusCache
    {
        Task InitResetCacheAsync();
        Task<Dictionary<int, string>?> GetProductStatusFromCacheOrCreateAsync();
    }
}
