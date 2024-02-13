using Microsoft.Extensions.DependencyInjection;
using ProductService.UseCases.Interfaces;

namespace ProductService.UseCases.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddUseCasesModule(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            return services;
        }
    }
}
