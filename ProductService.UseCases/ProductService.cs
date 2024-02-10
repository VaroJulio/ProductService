using SharedKernel.Interfaces;
using ProductService.UseCases.Dtos;
using ProductService.UseCases.Interfaces;
using ProductService.Domain.ProductAggregate;
using AutoMapper;
using ProductService.Domain.ProductAggregate.Specifications;

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

        public async Task<ProductDto?> UpdateProductAsync(UpdateProductDto updateProductDto, CancellationToken cancellationToken)
        {
            try
            {
                GetProductByIdSpec spec = new GetProductByIdSpec(updateProductDto.ProductId);
                var product = await repository.FirstOrDefaultAsync(spec);
                if (product != null)
                {
                    product.Update(updateProductDto.Name, updateProductDto.Stock, updateProductDto.Description,
                        updateProductDto.Price, updateProductDto.Status);
                    await repository.UpdateAsync(product, cancellationToken);                   
                }
                return mapper.Map<Product?, ProductDto?>(product);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
