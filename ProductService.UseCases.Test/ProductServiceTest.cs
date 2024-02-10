using AutoFixture.Xunit2;
using AutoMapper;
using Moq;
using ProductService.Domain.ProductAggregate;
using ProductService.UseCases.Dtos;
using ProductService.UseCases.Mappers;
using SharedKernel.Interfaces;

namespace ProductService.UseCases.Test
{
    public class ProductServiceTest
    {
        private readonly IMapper automapper;
        private MapperConfiguration mapperConfiguration;

        public ProductServiceTest()
        {
            mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<UseCasesProfileMapper>());
            automapper = new AutoMapper.Mapper(mapperConfiguration);
        }

        [Theory, AutoData]
        public async Task Should_Create_ProductAsync(CreateProductDto productDtoMock)
        {
            var newProduct = automapper.Map<Product>(productDtoMock);
            var newProductDto = automapper.Map<ProductDto>(productDtoMock);
            var productRepositoryMock = new Mock<IRepository<Product>>();
            productRepositoryMock.Setup(x => x.AddAsync(newProduct, default)).ReturnsAsync(newProduct);
            var productService = new ProductService(productRepositoryMock.Object, automapper);

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
            throw new NotImplementedException();
        }
    }
}