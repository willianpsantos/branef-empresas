using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Branef.Empresas.DB
{
    public class BranefWriteDbContextFactory : IDesignTimeDbContextFactory<BranefWriteDbContext>
    {
        public BranefWriteDbContext CreateDbContext(string[] args)
        {
            var configBuilder = new ConfigurationBuilder();
            var contextBuilder = new DbContextOptionsBuilder<BranefWriteDbContext>();

            var config = configBuilder.AddJsonFile("sharedsettings.json").Build();
            var connectionString = config.GetConnectionString("BranefDb");

            contextBuilder.UseSqlServer(connectionString);

            return new BranefWriteDbContext(contextBuilder.Options);
        }
    }
}
