namespace ProductService.Api.Endpoints.ProductEndpoints.GetById
{
    public class GetByIdProductResponse
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int Stock { get; set; }
        public  string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalPrice { get; set; }
    }
}
