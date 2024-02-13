namespace ProductService.UseCases.Interfaces
{
    public interface IProductStatusCache
    {
        Task InitResetCacheAsync();
        Task<Dictionary<int, string>?> GetProductStatusFromCacheOrCreateAsync();
    }
}
