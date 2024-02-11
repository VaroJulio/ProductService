using Microsoft.Extensions.Hosting;

namespace ProductService.UseCases.Interfaces
{
    public interface IHostedServiceWithExecutionCount : IHostedService
    {
        Task<int> GetExecutionCount();
    }
}
