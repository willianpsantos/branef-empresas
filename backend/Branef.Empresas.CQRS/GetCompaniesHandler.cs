using Branef.Empresas.DB;
using Branef.Empresas.Domain.Interfaces.Base;
using Branef.Empresas.Domain.Interfaces.Services;
using Branef.Empresas.Domain.Models;
using Branef.Empresas.Domain.Queries;
using MediatR;

namespace Branef.Empresas.CQRS
{
    public class GetCompaniesHandler : IRequestHandler<GetCompaniesQuery, IEnumerable<CompanyModel>>
    {
        private readonly ICompanyService<BranefReadDbContext> _companyService;

        public GetCompaniesHandler(
            ICompanyService<BranefReadDbContext> companyService
        )
        {
            _companyService = companyService;
        }

        public async Task<IEnumerable<CompanyModel>> Handle(
            GetCompaniesQuery request, 
            CancellationToken cancellationToken
        )
        {
            return await _companyService.GetAsync(request);
        }
    }
}
