namespace ProductService.UseCases.Dtos
{
    public class UpdateProductDto
    {
        public int ProductId { get; set; }
        public required string Name { get; set; }
        public int Stock { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public bool Status { get; set; }
    }
}
