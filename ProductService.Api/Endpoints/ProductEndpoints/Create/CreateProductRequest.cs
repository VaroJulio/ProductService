namespace ProductService.Api.Endpoints.ProductEndpoints.Create
{
    public class CreateProductRequest
    {
        public const string Route = "products";

        public string Name { get; set; } = string.Empty;
        public int Stock { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
