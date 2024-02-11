using Ardalis.Specification;
using AutoFixture.Xunit2;
using AutoMapper;
using Moq;
using ProductService.Domain.ProductAggregate;
using ProductService.UseCases.Dtos;
using ProductService.UseCases.Interfaces;
using ProductService.UseCases.Mappers;
using SharedKernel.Interfaces;
using FluentAssertions;

namespace ProductService.UseCases.Test
{
    public class ProductServiceTest
    {
        private readonly IMapper automapper;
        private readonly MapperConfiguration mapperConfiguration;

        public ProductServiceTest()
        {
            mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<UseCasesProfileMapper>());
            automapper = new Mapper(mapperConfiguration);
        }

        [Theory, AutoData]
        public async Task Should_Create_ProductAsync(CreateProductDto productDtoMock)
        {
            var newProduct = automapper.Map<Product>(productDtoMock);
            var newProductDto = automapper.Map<ProductDto>(productDtoMock);

            var productRepositoryMock = new Mock<IRepository<Product>>();
            var productStatusCacheMock = new Mock<IProductStatusCache>();
            var productDiscountRestClientMock = new Mock<IProductDiscountRestClient>();

            productRepositoryMock.Setup(x => x.AddAsync(newProduct, default)).ReturnsAsync(newProduct);

            var productService = new ProductService(productRepositoryMock.Object, productStatusCacheMock.Object,
                productDiscountRestClientMock.Object, automapper);

            var result = await productService.CreateProductAsync(productDtoMock, default);

            Assert.Multiple(() =>
            {
                Assert.NotNull(result);
                Assert.IsType<ProductDto>(result);
                Assert.Equal(newProductDto, result);
            });
        }

        [Theory, AutoData]
        public async Task Should_Update_ProductAsync(UpdateProductDto updateProductDtoMock)
        {
            var updatedProduct = automapper.Map<Product>(updateProductDtoMock);
            var updateProductDto = automapper.Map<UpdateProductDto>(updatedProduct);
            var updatedProductDto = automapper.Map<ProductDto>(updatedProduct);

            var productRepositoryMock = new Mock<IRepository<Product>>();
            var productStatusCacheMock = new Mock<IProductStatusCache>();
            var productDiscountRestClientMock = new Mock<IProductDiscountRestClient>();

            productRepositoryMock.Setup(x => x.FirstOrDefaultAsync(It.IsAny<ISpecification<Product>>(), default))
                .Returns(Task.FromResult(updatedProduct)!);
            productRepositoryMock.Setup(x => x.UpdateAsync(updatedProduct, default)).Returns(Task.FromResult(updatedProduct));

            var productService = new ProductService(productRepositoryMock.Object, productStatusCacheMock.Object,
                productDiscountRestClientMock.Object, automapper);

            var result = await productService.UpdateProductAsync(updateProductDto, default);

            Assert.Multiple(() =>
            {
                Assert.NotNull(result);
                Assert.IsType<ProductDto>(result);
                Assert.Equal(updatedProductDto, result);
            });
        }

        [Theory, AutoData]
        public async Task Should_Get_Product_By_Id_Async(Product product)
        {
            Dictionary<int, string>? productStatus = [];
            productStatus!.Add(0, "Inactive");
            productStatus!.Add(1, "Active");

            ProductDiscountResponseDto discountResponseDto = new() { Id = product.ProductId, DiscountRate = (decimal)(new Random().NextDouble() * 100) };

            var productRepositoryMock = new Mock<IRepository<Product>>();
            var productStatusCacheMock = new Mock<IProductStatusCache>();
            var productDiscountRestClientMock = new Mock<IProductDiscountRestClient>();

            productDiscountRestClientMock.Setup(x => x.GetProductDiscountAsync(product.ProductId, default)).ReturnsAsync(discountResponseDto);
            productStatusCacheMock.Setup(x => x.GetProductStatusFromCacheOrCreateAsync()).ReturnsAsync(productStatus);
            productRepositoryMock.Setup(x => x.FirstOrDefaultAsync(It.IsAny<ISpecification<Product>>(), default)).Returns(Task.FromResult(product)!);

            var productService = new ProductService(productRepositoryMock.Object, productStatusCacheMock.Object,
                productDiscountRestClientMock.Object, automapper);

            var detailedProduct = await productService.GetProductByIdAsync(product.ProductId, default);

            Assert.Multiple(() =>
            {
                Assert.NotNull(detailedProduct);
                Assert.IsType<DetailedProductDto>(detailedProduct);
                Assert.Equal(product.ProductId, detailedProduct.ProductId);
                detailedProduct.Status.Should().BeOneOf(new string[] { productStatus[0], productStatus[1] });
                detailedProduct.Discount.Should().Be(product.GetDiscount());
                detailedProduct.FinalPrice.Should().Be(product.CalculateFinalPrice());
            });
        }
    }
}