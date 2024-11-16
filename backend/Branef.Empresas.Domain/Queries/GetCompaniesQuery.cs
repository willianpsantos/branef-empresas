using Branef.Empresas.Domain.Models;
using MediatR;

namespace Branef.Empresas.Domain.Queries
{
    public class GetCompaniesQuery : 
        CompanyQuery, 
        IRequest<IEnumerable<CompanyModel>>
    {
    }
}
