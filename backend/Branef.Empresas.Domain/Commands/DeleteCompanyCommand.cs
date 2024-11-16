using MediatR;

namespace Branef.Empresas.Domain.Models
{
    public record DeleteCompanyCommand : IRequest<bool>
    {
        public DeleteCompanyCommand(Guid id) => Id = id;

        public Guid Id { get; set; }
    }
}
