using Branef.Empresas.DB;

namespace Branef.Empresas.Repositories
{
    public class CompanyReadRepository : CompanyRepository<BranefReadDbContext>
    {
        public CompanyReadRepository(BranefReadDbContext context) : base(context) { }
    }
}
