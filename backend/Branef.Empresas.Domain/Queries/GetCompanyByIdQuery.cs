using Branef.Empresas.Domain.Models;
using MediatR;

namespace Branef.Empresas.Domain.Queries
{
    public record GetCompanyByIdQuery : IRequest<CompanyModel>
    {
        public GetCompanyByIdQuery(Guid id) => Id = id;

        public Guid Id { get;set; }
    }
}
