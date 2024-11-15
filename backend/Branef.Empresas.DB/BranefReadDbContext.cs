using Branef.Empresas.DB.EntityConfigurations.ReadDb;
using Microsoft.EntityFrameworkCore;

namespace Branef.Empresas.DB
{
    public class BranefReadDbContext : BranefDbContext<BranefReadDbContext>
    {
        public BranefReadDbContext(DbContextOptions<BranefReadDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CompanyEntityConfiguration());
        }
    }
}
