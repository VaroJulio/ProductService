using Microsoft.Extensions.Configuration;
using Moq;
using ProductService.Infrastructure.RestClients;

namespace ProductService.Infrastructure.Test
{
    public class ProductDiscountRestClientTest
    {
        [Fact]
        public async Task Shoul_Get_ProductDiscountAsync() 
        {
            var productId = new Random().Next(0, 100);
            var configuration = new Mock<IConfiguration>();
            var restclient = new ProductDiscountRestClient(configuration.Object);
            var result = await restclient.GetProductDiscountAsync(productId, default);
            Assert.NotNull(result);
        }
    }
}
