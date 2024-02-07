using Ardalis.GuardClauses;

namespace ProductService.Domain.ProductAggregate
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public Product() 
        {
            Name = string.Empty;
            Description = string.Empty;
        }

        public Product(string name, int stock, string description, decimal price)
        {
            Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Stock = Guard.Against.Negative(stock, nameof(stock));
            Description = Guard.Against.NullOrWhiteSpace(description, nameof(description)); ;
            Price = Guard.Against.Negative(Guard.Against.Zero(price, nameof(price)), nameof(price));
            Status = true;
        }
    }
}
