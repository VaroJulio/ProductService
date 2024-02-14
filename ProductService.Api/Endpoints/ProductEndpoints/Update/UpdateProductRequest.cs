using FastEndpoints;

namespace ProductService.Api.Endpoints.ProductEndpoints.Update
{
    public class UpdateProductRequest
    {
        public const string Route = "products/{Id}";


        [BindFrom("Id")] public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Stock { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool Status { get; set; }
    }
}
