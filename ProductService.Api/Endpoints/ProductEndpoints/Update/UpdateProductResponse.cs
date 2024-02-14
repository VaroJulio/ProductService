namespace ProductService.Api.Endpoints.ProductEndpoints.Update
{
    public class UpdateProductResponse
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool Status { get; set; } = true;
        public int Stock { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
