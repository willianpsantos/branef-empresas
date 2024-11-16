using Branef.Empresas.DB;
using Branef.Empresas.Domain.Interfaces.Services;
using Branef.Empresas.Domain.Models;
using Branef.Empresas.Domain.Queries;
using MediatR;

namespace Branef.Empresas.CQRS
{
    public class GetCompanyByIdHandler : IRequestHandler<GetCompanyByIdQuery, CompanyModel>
    {
        private readonly ICompanyService<BranefReadDbContext> _companyService;

        public GetCompanyByIdHandler(
            ICompanyService<BranefReadDbContext> companyService
        )
        {
            _companyService = companyService;
        }

        public async Task<CompanyModel?> Handle(
            GetCompanyByIdQuery request, 
            CancellationToken cancellationToken
        )
        {
            return await _companyService.GetByIdAsync(request.Id);
        }
    }
}
