using Branef.Empresas.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Branef.Empresas.DB
{
    public abstract class BranefDbContext<TDbContext> : DbContext where TDbContext : DbContext
    {
        public BranefDbContext(DbContextOptions<TDbContext> options) : base(options)
        {

        }

        public DbSet<Company> Companies { get; set; }
    }
}
