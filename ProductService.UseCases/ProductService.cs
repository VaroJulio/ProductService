using SharedKernel.Interfaces;
using ProductService.UseCases.Dtos;
using ProductService.UseCases.Interfaces;
using ProductService.Domain.ProductAggregate;
using AutoMapper;

namespace ProductService.UseCases                          
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> repository;
        private readonly IMapper mapper;

        public ProductService(IRepository<Product> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto, CancellationToken cancellationToken)
        {
            try
            {
                Product newProduct = new(createProductDto.Name, createProductDto.Stock, createProductDto.Description, createProductDto.Price);
                var createdProduct = await repository.AddAsync(newProduct, cancellationToken);
                return mapper.Map<Product, ProductDto>(newProduct);
            }catch (Exception)
            {
                throw;
            }
        }

        public Task<IEnumerable<DetailedProductDto>> GetProductByIdAsync(int productId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ProductDto> UpdateProductAsync(UpdateProductDto updateProductDto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
