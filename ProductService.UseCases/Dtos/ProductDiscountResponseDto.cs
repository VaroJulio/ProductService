namespace ProductService.UseCases.Dtos
{
    public record ProductDiscountResponseDto
    {
        public int Id { get; set; }
        public decimal DiscountRate { get; set; }
    }
}
