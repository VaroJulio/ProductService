using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure.Data;

namespace ProductService.Infrastructure.Test
{

    public abstract class SqliteInMemoryDb : IDisposable
    {
        private const string ConnectionString = "DataSource=:memory:";
        private readonly SqliteConnection connection;
        protected readonly AppDbContext DbContext;

        protected SqliteInMemoryDb()
        {
            connection = new SqliteConnection(ConnectionString);
            connection.Open(); 
            
            var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseSqlite(connection)
                    .Options;

            DbContext = new AppDbContext(options);
            DbContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            connection.Close();
        }
    }
}
