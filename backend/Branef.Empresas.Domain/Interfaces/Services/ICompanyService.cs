using Branef.Empresas.Domain.Interfaces.Base;
using Branef.Empresas.Domain.Models;
using Branef.Empresas.Domain.Queries;

namespace Branef.Empresas.Domain.Interfaces.Services
{
    public interface ICompanyService<TDbContext> :
        IDomainGetAsyncService<CompanyQuery, CompanyModel>,
        IDomainGetPaginatedAsyncService<CompanyQuery, CompanyModel>,
        IDomainGetByIdAsyncService<CompanyModel>,
        IDomainInsertAsyncService<InsertOrUpdateCompanyCommand, CompanyModel>,
        IDomainUpdateService<InsertOrUpdateCompanyCommand, CompanyModel>,
        IDomainDeleteAsyncService,
        IDomainSaveChangesAsyncService

        where TDbContext : class
    {
    }
}
