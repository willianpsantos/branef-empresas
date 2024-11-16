using Branef.Empresas.DB;
using Branef.Empresas.Domain.Interfaces.Base;
using Branef.Empresas.Domain.Interfaces.Services;
using Branef.Empresas.Domain.Models;
using Branef.Empresas.Domain.Queries;
using MediatR;

namespace Branef.Empresas.CQRS
{
    public class GetPaginatedCompaniesHandler : IRequestHandler<GetPaginatedCompaniesQuery, IPaginatedResponse<CompanyModel>>
    {
        private readonly ICompanyService<BranefReadDbContext> _companyService;

        public GetPaginatedCompaniesHandler(
            ICompanyService<BranefReadDbContext> companyService
        )
        {
            _companyService = companyService;
        }

        public async Task<IPaginatedResponse<CompanyModel>> Handle(
            GetPaginatedCompaniesQuery request, 
            CancellationToken cancellationToken
        )
        {
            return await _companyService.GetPaginatedAsync(
                request.Page ?? 0,
                request.PageSize ?? 0,
                request
            );
        }
    }
}
