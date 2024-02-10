using Ardalis.Specification;

namespace ProductService.Domain.ProductAggregate.Specifications
{
    public class GetProductByIdSpec  : Specification<Product>
    {
        public GetProductByIdSpec(int productId) 
        { 
            Query.Where(x => x.ProductId == productId);
        }
    }
}
