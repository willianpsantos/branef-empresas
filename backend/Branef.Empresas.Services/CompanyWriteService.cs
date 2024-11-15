using Branef.Empresas.Data.Entities;
using Branef.Empresas.DB;
using Branef.Empresas.Domain.Interfaces.Base;
using Branef.Empresas.Domain.Interfaces.Converters;
using Branef.Empresas.Domain.Queries;

namespace Branef.Empresas.Services
{
    public class CompanyWriteService : CompanyService<BranefWriteDbContext>
    {
        public CompanyWriteService(
            IRepository<Company, BranefWriteDbContext> repository,
            ICompanyDomainConverter domainConverter,
            IQueryToExpressionAdapter<CompanyQuery, Company> queryAdapter
        )
        : base(repository, domainConverter, queryAdapter)
        {

        }
    }
}
