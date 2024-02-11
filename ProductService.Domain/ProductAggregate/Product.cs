using Ardalis.GuardClauses;
using SharedKernel.Interfaces;

namespace ProductService.Domain.ProductAggregate
{
    public class Product : IAggregateRoot
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        private string StatusName = string.Empty;
        private decimal Discount = 0;

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

        public void Update(string? name, int? stock, string? description, decimal? price, bool? status)
        {
            Name = name ?? Name;
            Stock = stock ?? Stock;
            Description = description ?? Description;
            Price = price ?? Price;
            Status = status ?? Status;
        }

        public void SetStatusName(string statusName) => StatusName = statusName;
        public void SetDiscount(decimal discount) => Discount = discount;
        public string GetStatusName() => StatusName;
        public decimal GetDiscount() => Discount;
        public decimal CalculateFinalPrice() => Price * (100 - Discount) / 100;
    }
}
