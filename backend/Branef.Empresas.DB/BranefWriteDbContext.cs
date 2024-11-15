using Branef.Empresas.DB.EntityConfigurations.WriteDb;
using Microsoft.EntityFrameworkCore;

namespace Branef.Empresas.DB
{
    public class BranefWriteDbContext : BranefDbContext<BranefWriteDbContext>
    {
        public BranefWriteDbContext(DbContextOptions<BranefWriteDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CompanyEntityConfiguration());
        }
    }
}
