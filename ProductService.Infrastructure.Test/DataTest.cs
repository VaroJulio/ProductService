using AutoFixture.Xunit2;
using ProductService.Domain.ProductAggregate;
using ProductService.Domain.ProductAggregate.Specifications;
using ProductService.Infrastructure.Data;
using SharedKernel.Interfaces;

namespace ProductService.Infrastructure.Test
{
    public class DataTest  : SqliteInMemoryDb
    {
        [Fact]
        public async Task Should_Connect_To_Db()
        {
            Assert.True(await DbContext.Database.CanConnectAsync());
        }

        [Theory, AutoData]
        public async Task Should_Add_Product_To_Db(Product product)
        {
            IRepository<Product> productRespotitory = new EfRepository<Product>(DbContext);

            var result = await productRespotitory.AddAsync(product);

            Assert.Multiple(() =>
            {
                Assert.NotNull(result);
                Assert.Equal(1, DbContext.Products.Count());
            });
        }

        [Theory, AutoData]
        public async Task Should_Add_Get_Product_By_Id_From_Db(Product product)
        {
            IRepository<Product> productRespotitory = new EfRepository<Product>(DbContext);
            
            var newProductResult = await productRespotitory.AddAsync(product);
            var getProductByIdResult = await productRespotitory.GetByIdAsync(product.ProductId);

            Assert.Multiple(() =>
            {
                Assert.NotNull(getProductByIdResult);
                Assert.Equal(newProductResult, getProductByIdResult);
                Assert.Equal(1, DbContext.Products.Count());
            });
        }

        [Theory, AutoData]
        public async Task Should_Update_Product_In_Db(string name, int stock, string description, decimal price, bool status)
        {
            var newProduct = new Product("Cheese", 5, "Keep into the freezer", (decimal) 11.854);
            IRepository<Product> productRespotitory = new EfRepository<Product>(DbContext);
            
            var newProductResult = await productRespotitory.AddAsync(newProduct);
            var getProductByIdResult = await productRespotitory.GetByIdAsync(newProduct.ProductId);            
            getProductByIdResult!.Update(name: name, stock: stock, description: description, price: price, status: status);
            await productRespotitory.UpdateAsync(getProductByIdResult);
            getProductByIdResult = await productRespotitory.GetByIdAsync(newProduct.ProductId);

            Assert.Multiple(() =>
            {
                Assert.NotNull(getProductByIdResult);
                Assert.Equal(name, getProductByIdResult.Name);
                Assert.Equal(stock, getProductByIdResult.Stock);
                Assert.Equal(description, getProductByIdResult.Description);
                Assert.Equal(price, getProductByIdResult.Price);
                Assert.Equal(status, getProductByIdResult.Status);
                Assert.Equal(1, DbContext.Products.Count());
            });
        }

        [Theory, AutoData]
        public async Task Should_Filter_By_Id_Usig_Specification_In_Db(Product product)
        {
            IRepository<Product> productRespotitory = new EfRepository<Product>(DbContext);
            GetProductByIdSpec spec = new(product.ProductId);
            
            var newProductResult = await productRespotitory.AddAsync(product);
            var getProductByIdResult = await productRespotitory.FirstOrDefaultAsync(spec, default);

            Assert.Multiple(() =>
            {
                Assert.NotNull(getProductByIdResult);
                Assert.Equal(newProductResult, getProductByIdResult);
                Assert.Equal(1, DbContext.Products.Count());
            });
        }
    }
}
