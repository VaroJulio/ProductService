namespace ProductService.Api.Endpoints.ProductEndpoints.GetById
{
    public class GetByIdProductRequest
    {
        public const string Route = "products/{Id}";

        public int Id { get; set; }
    }
}
