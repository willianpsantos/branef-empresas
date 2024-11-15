using Branef.Empresas.DB;

namespace Branef.Empresas.Repositories
{
    public class CompanyWriteRepository : CompanyRepository<BranefWriteDbContext>
    {
        public CompanyWriteRepository(BranefWriteDbContext context) : base(context) { }
    }
}
