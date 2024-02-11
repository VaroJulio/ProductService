using Microsoft.Extensions.Configuration;
using ProductService.UseCases.Dtos;
using ProductService.UseCases.Interfaces;
using RestSharp;

namespace ProductService.Infrastructure.RestClients
{
    public class ProductDiscountRestClient : IProductDiscountRestClient
    {
        private readonly IRestClient restClient;
        private readonly IConfiguration configuration;

        public ProductDiscountRestClient(IConfiguration config)
        {
            configuration = config;
            var baseUrl = config["GetProductDiscountUrl"];
            var options = new RestClientOptions("https://65c7e9aee7c384aada6f1485.mockapi.io/");
            restClient = new RestClient(options);
        }

        public async Task<ProductDiscountResponseDto?> GetProductDiscountAsync(int productId, CancellationToken cancellationToken)
        {
            try
            {
                var request = new RestRequest($"api/v1/GetDiscount/{productId}");
                var response = await restClient.GetAsync<ProductDiscountResponseDto>(request, cancellationToken);
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
