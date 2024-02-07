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
        [InlineData("Bottle of Water x 500 ml", 0, "A pepsico product", 11.58)]
        public void Should_Create_A_Product_Using_Parameters(string name, int stock, string description, decimal price)
        {
            throw new NotImplementedException();
        }
    }
}