namespace ProductService.UseCases.Dtos
{
    public record ProductDto
    {
        public int ProductId { get; set; }
        public required string Name { get; set; }
        public bool Status { get; set; } = true;
        public int Stock { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
    }
}
