using FluentAssertions;
using ProductService.Domain.ProductAggregate;

namespace ProductService.Domain.Test
{
    public class ProductTest
    {
        private readonly decimal testPrice = decimal.Parse("89.45263");

        [Fact]
        public void Should_Create_A_Product_Using_Default_Ctr()
        {
            Product newProduct;

            newProduct = new Product();

            Assert.Multiple(() =>
            {
                Assert.NotNull(newProduct);
                Assert.IsType<Product>(newProduct);
            });
        }

        [Theory]
        [InlineData("Cheese", 5, "Keep into the freezer", 11.58)]
        [InlineData("T-bone steak", 8, "Pretty delicious", 30)]
        [InlineData("Spagetti", 12, "Measured in pounds", 24)]
        [InlineData("Bottle of Water x 500 ml", 0, "A pepsico product", 2.50)]
        public void Should_Create_A_Product_Using_Parameters(string name, int stock, string description, decimal price)
        {
            Product newProduct;

            newProduct = new Product(name, stock, description, price);

            Assert.Multiple(() =>
            {
                Assert.NotNull(newProduct);
                Assert.IsType<Product>(newProduct);
            });

            Assert.Multiple(() =>
            {
                Assert.Equal(default, newProduct.ProductId);
                Assert.Equal(name, newProduct.Name);
                Assert.True(newProduct.Status);
                Assert.Equal(stock, newProduct.Stock);
                Assert.Equal(description, newProduct.Description);
                Assert.Equal(price, newProduct.Price);
            });
        }

        [Theory]
        [InlineData(null, 5, "custom description", 3)]
        [InlineData("custom name", 5, null, 3)]
        [InlineData("custom name", 5, "     ", 3)]
        [InlineData("     ", 5, "custom description", 3)]
        [InlineData("custom name", -700, "custom description", 3)]
        [InlineData("custom name", 5, "custom description", -856.52)]
        [InlineData("custom name", 5, "custom description", 0)]
        public void Should_Not_Create_A_Product_Using_Wrong_Parameters_Values(string? name, int stock, string? description, decimal price)
        {
            Product? newProduct = null;

            try
            {
                newProduct = new Product(name!, stock, description!, price);
            }
            catch (Exception ex)
            {
                Assert.Multiple(() =>
                {
                    Assert.NotNull(ex.Message);
                    ex.GetType().Should().Match(x => x.Name == typeof(ArgumentException).Name || x.Name == typeof(ArgumentNullException).Name);
                });
            }

            Assert.Null(newProduct);
        }


        [Theory]
        [InlineData("Juan Valdez Cofee", 5, "The best cofee", "18.759",  false)]
        [InlineData(null, 5, "The best cofee", "18.759", false)]
        [InlineData("Juan Valdez Cofee", null, "The best cofee", "18.759", false)]
        [InlineData("Juan Valdez Cofee", 5, null, "18.759", false)]
        [InlineData("Juan Valdez Cofee", 5, "The best cofee", null, false)]
        [InlineData("Juan Valdez Cofee", 5, "The best cofee", "18.759", null)]
        [InlineData(null, null, null, null, null)]
        public void Should_Update_A_Product(string? name, int? stock, string? description, string? price, bool? status)
        {
            var newProduct = new Product("A", 1, "A", 1);
            
            newProduct.Update(name, stock, description, (price != null) ? decimal.Parse(price) : null, status);

            Assert.Multiple(() =>
            { 
                Assert.Equal(default, newProduct.ProductId);

                if (name is not null)                   
                    Assert.Equal(name, newProduct.Name);
                if (stock is not null)
                    Assert.Equal(stock, newProduct.Stock);
                if (description is not null)
                    Assert.Equal(description, newProduct.Description);
                if (price is not null)
                    Assert.Equal(decimal.Parse(price), newProduct.Price);
                if (status is not null)
                    Assert.True(status == newProduct.Status);
            });
        }
    }
}