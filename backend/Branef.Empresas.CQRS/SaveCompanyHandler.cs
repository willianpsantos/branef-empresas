using Branef.Empresas.DB;
using Branef.Empresas.Domain.Interfaces.Services;
using Branef.Empresas.Domain.Models;
using MediatR;

namespace Branef.Empresas.CQRS
{
    public class SaveCompanyHandler : IRequestHandler<InsertOrUpdateCompanyCommand, CompanyModel>
    {
        private readonly ICompanyService<BranefWriteDbContext> _companyService;
        private readonly ICompanyReplicationService _companyReplicationService;

        public SaveCompanyHandler(
            ICompanyService<BranefWriteDbContext> companyService,
            ICompanyReplicationService companyReplicationService
        )
        {
            _companyService = companyService;
            _companyReplicationService = companyReplicationService;
        }

        public async Task<CompanyModel> Handle(
            InsertOrUpdateCompanyCommand request, 
            CancellationToken cancellationToken
        )
        {
            var company = 
                request.Id is null || request.Id == Guid.Empty
                    ? await _companyService.InsertAsync(request)
                    : _companyService.Update(request.Id!.Value, request);

            await _companyService.SaveChangesAsync();

            await _companyReplicationService.ReplicateChangesAsync(company);

            return company;
        }
    }
}
