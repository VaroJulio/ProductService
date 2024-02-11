using AutoMapper;
using ProductService.Domain.ProductAggregate;
using ProductService.Domain.ProductAggregate.Specifications;
using ProductService.UseCases.Dtos;
using ProductService.UseCases.Interfaces;
using SharedKernel.Interfaces;

namespace ProductService.UseCases
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> repository;
        private readonly IProductStatusCache productStatusCache;
        private readonly IProductDiscountRestClient productDiscountRestClient;
        private readonly IMapper mapper;

        public ProductService(IRepository<Product> repository, IProductStatusCache productStatusCache,
            IProductDiscountRestClient productDiscountRestClient, IMapper mapper)
        {
            this.repository = repository;
            this.productStatusCache = productStatusCache;
            this.productDiscountRestClient = productDiscountRestClient;
            this.mapper = mapper;
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto, CancellationToken cancellationToken)
        {
            try
            {
                Product newProduct = new(createProductDto.Name, createProductDto.Stock, createProductDto.Description, createProductDto.Price);
                var createdProduct = await repository.AddAsync(newProduct, cancellationToken);
                return mapper.Map<ProductDto>(newProduct);
            }catch (Exception)
            {
                throw;
            }
        }

        public async Task<DetailedProductDto?> GetProductByIdAsync(int productId, CancellationToken cancellationToken)
        {
            try
            {
                DetailedProductDto? detailedProduct = default;
                GetProductByIdSpec spec = new(productId);
                var product = await repository.FirstOrDefaultAsync(spec, cancellationToken);

                if (product is not null)
                {
                    var productStatus = await productStatusCache.GetProductStatusFromCacheOrCreateAsync();
                    
                    if (productStatus is not null)
                    {
                        GetProductStatusName(product.Status, productStatus, out string statusName);
                        product.SetStatusName(statusName);
                    }
                                            
                    var discount = await productDiscountRestClient.GetProductDiscountAsync(product.ProductId, cancellationToken);
                    product.SetDiscount(discount?.DiscountRate ?? 0);

                    var productDetail = mapper.Map<DetailedProductDto>(product);
                    detailedProduct = EnrichDetailedProductDtoFromProduct(productDetail, product);
                }

                return detailedProduct;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ProductDto?> UpdateProductAsync(UpdateProductDto updateProductDto, CancellationToken cancellationToken)
        {
            try
            {
                GetProductByIdSpec spec = new(updateProductDto.ProductId);
                var product = await repository.FirstOrDefaultAsync(spec, default);

                if (product is not null)
                {
                    product.Update(updateProductDto.Name, updateProductDto.Stock, updateProductDto.Description,
                        updateProductDto.Price, updateProductDto.Status);
                    await repository.UpdateAsync(product, cancellationToken);                   
                }

                return mapper.Map<ProductDto?>(product);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void GetProductStatusName(bool status, Dictionary<int, string> productStatus, out string statusName)
        {
            var statusCode = status ? 1 : 0;
            productStatus.TryGetValue(statusCode, out string? nameOfTheStatus);
            statusName = nameOfTheStatus ?? string.Empty;
        }

        private static DetailedProductDto EnrichDetailedProductDtoFromProduct(DetailedProductDto detailedProductDto, Product product)
        {
            detailedProductDto.Status = product.GetStatusName();
            detailedProductDto.Discount = product.GetDiscount();
            detailedProductDto.FinalPrice = product.CalculateFinalPrice();
            return detailedProductDto;
        }
    }
}
