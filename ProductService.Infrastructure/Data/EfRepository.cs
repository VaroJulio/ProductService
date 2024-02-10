using Ardalis.Specification.EntityFrameworkCore;
using SharedKernel.Interfaces;

namespace ProductService.Infrastructure.Data
{
    public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot 
    {
        public EfRepository(AppDbContext dbContext) : base(dbContext) { }
    }
}
