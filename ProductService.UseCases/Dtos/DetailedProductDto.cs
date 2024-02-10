namespace ProductService.UseCases.Dtos
{
    public record DetailedProductDto
    {
        public int ProductId { get; set; }
        public required string Name { get; set; }
        public required string Status { get; set; }
        public int Stock { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalPrice { get; set; }
    }
}
