using AutoFixture.Xunit2;
using AutoMapper;
using FastEndpoints;
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http;
using Moq;
using ProductService.Api.Endpoints.ProductEndpoints.Create;
using ProductService.UseCases.Dtos;
using ProductService.UseCases.Interfaces;
using ProductService.UseCases.Mappers;

namespace ProductService.Api.Test.ProductEndpoints
{
    public class CreateProductTest
    {
        private CreateProductRequest request;
        private CreateProductRequestValidator validator;
        private MapperConfiguration configMapper;
        private AutoMapper.IMapper autoMapper;
        private CreateProductDto createProduct;
        private ProductDto product;
        private Mock<IProductService> productServiceMock;

        public CreateProductTest()
        {
            request = new();
            validator = new();
            configMapper = new MapperConfiguration(cfg =>
                                cfg.AddProfiles(new List<Profile>()
                                {
                                                new CreateProductMapper(),
                                                new UseCasesProfileMapper()
                                }));
            autoMapper = new Mapper(configMapper);
            createProduct = autoMapper.Map<CreateProductDto>(request);
            product = autoMapper.Map<ProductDto>(createProduct);
            productServiceMock = new();
        }

        [Theory, AutoData]
        public async Task Should_Create_Product(CreateProductRequest request)
        {
            //Arrange
            ArrangeTest(ref request, ref createProduct, ref product, ref productServiceMock);
            productServiceMock.Setup(x => x.CreateProductAsync(createProduct, CancellationToken.None)).ReturnsAsync(product);         
            var endpoint = Factory.Create<Create>(productServiceMock.Object, autoMapper);

            //Act
            validator.TestValidate(request).ShouldNotHaveAnyValidationErrors();
            await endpoint.HandleAsync(request, CancellationToken.None);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.NotNull(endpoint.Response);
                Assert.Equal(StatusCodes.Status201Created, endpoint.HttpContext.Response.StatusCode);
            });
        }

        [Theory, AutoData]
        public void Should_Not_Create_Product(CreateProductRequest request)
        {
            //Arrange
            ArrangeTest(ref request, ref createProduct, ref product, ref productServiceMock);

            productServiceMock.Setup(x => x.CreateProductAsync(createProduct, CancellationToken.None))
                .ThrowsAsync(new ArgumentOutOfRangeException());

            var endpoint = Factory.Create<Create>(productServiceMock.Object, autoMapper);

            //Act
            validator.TestValidate(request).ShouldNotHaveAnyValidationErrors();

            //Assert
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => { await endpoint.HandleAsync(request, CancellationToken.None); });
        }

        [Theory, AutoData]
        public void Should_Have_Validation_Errors(CreateProductRequest request)
        {
            //Arrange
            request.Name = "qqqqqqqqqweeeeeeeefdfffffffffffffssssssssssssssssssssssssffffffffffffffffffsssssssssssssssssssssqqqqqqqqqqqqqq";
            
            ArrangeTest(ref request, ref createProduct, ref product, ref productServiceMock);

            productServiceMock.Setup(x => x.CreateProductAsync(createProduct, CancellationToken.None))
                .ThrowsAsync(new ArgumentOutOfRangeException());

            var endpoint = Factory.Create<Create>(productServiceMock.Object, autoMapper);

            //Act
            var result = validator.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        private void ArrangeTest(
            ref CreateProductRequest request,
            ref CreateProductDto createProduct,
            ref ProductDto product,
            ref Mock<IProductService> productServiceMock
        )
        {
            createProduct = autoMapper.Map<CreateProductDto>(request);
            product = autoMapper.Map<ProductDto>(createProduct);
            productServiceMock = new();
        }
    }
}
