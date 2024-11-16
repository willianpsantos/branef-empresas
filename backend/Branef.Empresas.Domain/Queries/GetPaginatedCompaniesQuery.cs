using Branef.Empresas.Domain.Interfaces.Base;
using Branef.Empresas.Domain.Models;
using MediatR;

namespace Branef.Empresas.Domain.Queries
{
    public class GetPaginatedCompaniesQuery : 
        CompanyQuery, 
        IRequest<IPaginatedResponse<CompanyModel>>
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
    }
}
