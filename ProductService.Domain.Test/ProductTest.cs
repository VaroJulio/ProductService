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
    }
}