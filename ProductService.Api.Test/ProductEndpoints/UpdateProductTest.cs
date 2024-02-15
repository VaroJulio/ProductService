using AutoFixture.Xunit2;
using AutoMapper;
using FastEndpoints;
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http;
using Moq;
using ProductService.Api.Endpoints.ProductEndpoints.Create;
using ProductService.Api.Endpoints.ProductEndpoints.Update;
using ProductService.UseCases.Dtos;
using ProductService.UseCases.Interfaces;
using ProductService.UseCases.Mappers;

namespace ProductService.Api.Test.ProductEndpoints
{
    public class UpdateProductTest
    {
        private UpdateProductRequest request;
        private UpdateProductRequestValidator validator;
        private MapperConfiguration configMapper;
        private AutoMapper.IMapper autoMapper;
        private UpdateProductDto updateProduct;
        private Mock<IProductService> productServiceMock;

        public UpdateProductTest()
        {
            request = new();
            validator = new();
            configMapper = new MapperConfiguration(cfg =>
                                cfg.AddProfiles(new List<Profile>()
                                {
                                    new UpdateProductMapper(),
                                    new UseCasesProfileMapper()
                                }));
            autoMapper = new Mapper(configMapper);
            updateProduct = autoMapper.Map<UpdateProductDto>(request);
            productServiceMock = new();
        }

        [Theory, AutoData]
        public async Task Should_Update_Product(ProductDto product)
        {
            ArrangeTest(ref request, ref product, ref updateProduct, ref productServiceMock);
            productServiceMock.Setup(x => x.UpdateProductAsync(It.IsAny<UpdateProductDto>(), default)).ReturnsAsync(product);
            var endpoint = Factory.Create<Update>(productServiceMock.Object, autoMapper);

            //Act
            validator.TestValidate(request).ShouldNotHaveAnyValidationErrors();
            await endpoint.HandleAsync(request, CancellationToken.None);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.NotNull(endpoint.Response);
                Assert.Equal(StatusCodes.Status200OK, endpoint.HttpContext.Response.StatusCode);
            });
        }

        [Theory, AutoData]
        public void Should_Not_Update_Product(ProductDto product)
        {
            //Arrange
            ArrangeTest(ref request, ref product, ref updateProduct, ref productServiceMock);

            productServiceMock.Setup(x => x.UpdateProductAsync(It.IsAny<UpdateProductDto>(), default))
                .ThrowsAsync(new ArgumentOutOfRangeException());

            var endpoint = Factory.Create<Update>(productServiceMock.Object, autoMapper);

            //Act
            validator.TestValidate(request).ShouldNotHaveAnyValidationErrors();

            //Assert
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => { await endpoint.HandleAsync(request, CancellationToken.None); });
        }

        [Theory, AutoData]
        public void Should_Have_Validation_Errors(ProductDto product)
        {
            //Arrange
            ArrangeTest(ref request, ref product, ref updateProduct, ref productServiceMock);
            request.Name = "qqqqqqqqqweeeeeeeefdfffffffffffffssssssssssssssssssssssssffffffffffffffffffsssssssssssssssssssssqqqqqqqqqqqqqqdsfsdfsd";

            productServiceMock.Setup(x => x.UpdateProductAsync(updateProduct, CancellationToken.None))
                .ThrowsAsync(new ArgumentOutOfRangeException());

            var endpoint = Factory.Create<Create>(productServiceMock.Object, autoMapper);

            //Act
            var result = validator.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        private void ArrangeTest(
            ref UpdateProductRequest request,
            ref ProductDto product,
            ref UpdateProductDto updateProduct,
            ref Mock<IProductService> productServiceMock
)
        {
            request = autoMapper.Map<UpdateProductRequest>(product);
            updateProduct = autoMapper.Map<UpdateProductDto>(request);
            productServiceMock = new();
        }
    }
}
