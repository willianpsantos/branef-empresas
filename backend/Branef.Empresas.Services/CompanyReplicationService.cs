using Branef.Empresas.Domain.EventMessages;
using Branef.Empresas.Domain.Interfaces.Services;
using Branef.Empresas.Domain.Models;
using MassTransit;

namespace Branef.Empresas.Services
{
    public class CompanyReplicationService : ICompanyReplicationService
    {
        private readonly IPublishEndpoint _bus;

        public CompanyReplicationService(IPublishEndpoint bus)
        {
            _bus = bus;
        }

        public async ValueTask ReplicateChangesAsync(CompanyModel company)
        {
            await _bus.Publish<CompanyAddedOrUpdateEventMessage>(new CompanyAddedOrUpdateEventMessage
            {
                Id = company.Id,
                Name = company.Name,
                Size = company.Size,
                IncludedOrUpdatedAt = DateTimeOffset.UtcNow
            });
        }

        public async ValueTask ReplicateDeletedASync(Guid companyId)
        {
            await _bus.Publish(new CompanyDeleteEventMessage
            {
                Id= companyId,
                DeletedAt = DateTimeOffset.UtcNow
            });
        }
    }
}
