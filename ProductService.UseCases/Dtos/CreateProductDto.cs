namespace ProductService.UseCases.Dtos
{
    public record CreateProductDto
    {
        public required string Name { get; set; }
        public int Stock { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
    }
}
