using Microsoft.Extensions.DependencyInjection;
using ProductService.Domain.ProductAggregate;
using ProductService.Infrastructure.Cache;
using ProductService.Infrastructure.Data;
using ProductService.Infrastructure.HostedServices;
using ProductService.Infrastructure.RestClients;
using ProductService.UseCases.Interfaces;
using SharedKernel.Interfaces;

namespace ProductService.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInfrastructureModule(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<IProductStatusCache, ProductStatusCache>();
            services.AddScoped<IRepository<Product>, EfRepository<Product>>();
            services.AddScoped<IReadRepository<Product>, EfRepository<Product>>();
            services.AddTransient<IProductDiscountRestClient, ProductDiscountRestClient>();
            services.AddHostedService<CacheTimedHostedService>();
            return services;
        }
    }
}
