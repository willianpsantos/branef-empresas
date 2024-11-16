using Branef.Empresas.DB;
using Branef.Empresas.Domain.Interfaces.Services;
using Branef.Empresas.Domain.Models;
using MediatR;

namespace Branef.Empresas.CQRS
{
    public class DeleteCompanyHandler : IRequestHandler<DeleteCompanyCommand, bool>
    {
        private readonly ICompanyService<BranefWriteDbContext> _companyService;
        private readonly ICompanyReplicationService _companyReplicationService;

        public DeleteCompanyHandler(
            ICompanyService<BranefWriteDbContext> companyService,
            ICompanyReplicationService companyReplicationService
        )
        {
            _companyService = companyService;
            _companyReplicationService = companyReplicationService;
        }

        public async Task<bool> Handle(
            DeleteCompanyCommand request, 
            CancellationToken cancellationToken
        )
        {
            var success = await _companyService.DeleteAsync(request.Id);

            await _companyService.SaveChangesAsync();

            if (success)
                await _companyReplicationService.ReplicateDeletedASync(request.Id);

            return success;
        }
    }
}
