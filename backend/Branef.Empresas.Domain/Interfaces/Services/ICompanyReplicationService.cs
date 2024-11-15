using Branef.Empresas.Domain.Models;

namespace Branef.Empresas.Domain.Interfaces.Services
{
    public interface ICompanyReplicationService
    {
        ValueTask ReplicateChangesAsync(CompanyModel company);
        ValueTask ReplicateDeletedASync(Guid companyId);
    }
}
