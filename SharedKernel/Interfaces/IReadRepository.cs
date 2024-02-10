using Ardalis.Specification;

namespace SharedKernel.Interfaces
{
    public interface IReadRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
    {
    }
}