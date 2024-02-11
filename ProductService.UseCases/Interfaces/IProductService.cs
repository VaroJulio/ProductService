using ProductService.UseCases.Dtos;

namespace ProductService.UseCases.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto, CancellationToken cancellationToken);
        Task<ProductDto?> UpdateProductAsync(UpdateProductDto updateProductDto, CancellationToken cancellationToken);
        Task<DetailedProductDto?> GetProductByIdAsync(int productId, CancellationToken cancellationToken);
    }
}
