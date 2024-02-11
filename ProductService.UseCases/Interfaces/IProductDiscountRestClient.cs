using ProductService.UseCases.Dtos;

namespace ProductService.UseCases.Interfaces
{
    public interface IProductDiscountRestClient
    {
        Task<ProductDiscountResponseDto?> GetProductDiscountAsync(int productId, CancellationToken cancellation);
    }
}
