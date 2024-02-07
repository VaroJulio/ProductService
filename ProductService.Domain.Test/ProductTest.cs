using FluentAssertions;
using ProductService.Domain.ProductAggregate;

namespace ProductService.Domain.Test
{
    public class ProductTest
    {
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
    }
}