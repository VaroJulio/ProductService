using AutoFixture.Xunit2;
using AutoMapper;
using FastEndpoints;
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http;
using Moq;
using ProductService.Api.Endpoints.ProductEndpoints.GetById;
using ProductService.UseCases.Dtos;
using ProductService.UseCases.Interfaces;
using ProductService.UseCases.Mappers;

namespace ProductService.Api.Test.ProductEndpoints
{
    public class GetByIdProductTest
    {
        private GetByIdProductRequest request;
        private GetByIdProductRequestValidator validator;
        private MapperConfiguration configMapper;
        private AutoMapper.IMapper autoMapper;
        private Mock<IProductService> productServiceMock;

        public GetByIdProductTest()
        {
            request = new();
            validator = new();
            configMapper = new MapperConfiguration(cfg =>
                                cfg.AddProfiles(new List<Profile>()
                                {
                                                new GetByIdProductMapper(),
                                                new UseCasesProfileMapper()
                                }));
            autoMapper = new Mapper(configMapper);
            productServiceMock = new();
        }

        [Theory, AutoData]
        public async Task Should_Get_Product_By_Id(DetailedProductDto product)
        {
            //Arrange
            ArrangeTest(ref request, ref product, ref productServiceMock);
            productServiceMock.Setup(x => x.GetProductByIdAsync(request.Id, CancellationToken.None)).ReturnsAsync(product);
            var endpoint = Factory.Create<GetById>(productServiceMock.Object, autoMapper);

            //Act
            validator.TestValidate(request).ShouldNotHaveAnyValidationErrors();
            await endpoint.HandleAsync(request, CancellationToken.None);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.NotNull(endpoint.Response);
                Assert.Equal(autoMapper.Map<GetByIdProductResponse>(product).ProductId, endpoint.Response.ProductId);
                Assert.Equal(StatusCodes.Status200OK, endpoint.HttpContext.Response.StatusCode);
            });
        }

        [Theory, AutoData]
        public async Task Should_Not_Get_Product_By_Id(DetailedProductDto product)
        {
            //Arrange
            DetailedProductDto? notFoundPRoduct = null;
            ArrangeTest(ref request, ref product, ref productServiceMock);
            productServiceMock.Setup(x => x.GetProductByIdAsync(request.Id, CancellationToken.None)).ReturnsAsync(notFoundPRoduct);
            var endpoint = Factory.Create<GetById>(productServiceMock.Object, autoMapper);

            //Act
            validator.TestValidate(request).ShouldNotHaveAnyValidationErrors();
            await endpoint.HandleAsync(request, CancellationToken.None);

            //Assert
            Assert.Equal(StatusCodes.Status404NotFound, endpoint.HttpContext.Response.StatusCode);
        }

        [Theory, AutoData]
        public void Should_Have_Validation_Errors(DetailedProductDto product)
        {
            //Arrange
            product.ProductId = -10;
            ArrangeTest(ref request, ref product, ref productServiceMock);
            productServiceMock.Setup(x => x.GetProductByIdAsync(product.ProductId, CancellationToken.None)).ThrowsAsync(new ArgumentException());
            var endpoint = Factory.Create<GetById>(productServiceMock.Object, autoMapper);

            //Act
            var result = validator.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        private void ArrangeTest(
            ref GetByIdProductRequest request,
            ref DetailedProductDto product,
            ref Mock<IProductService> productServiceMock
)
        {
            request.Id = product.ProductId;
            productServiceMock = new();
        }
    }
}
