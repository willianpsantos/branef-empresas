using Branef.Empresas.DB;
using Branef.Empresas.Domain.EventMessages;
using Branef.Empresas.Domain.Interfaces.Services;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Branef.Empresas.Events
{
    public class CompanyDeletedEventConsumer : IConsumer<CompanyDeleteEventMessage>
    {
        private readonly ILogger<CompanyDeletedEventConsumer> _logger;
        private readonly ICompanyService<BranefReadDbContext> _companyService;

        public CompanyDeletedEventConsumer(
            ILogger<CompanyDeletedEventConsumer> logger,
            ICompanyService<BranefReadDbContext> companyService
        )
        {
            _logger = logger;
            _companyService = companyService;
        }

        public async Task Consume(ConsumeContext<CompanyDeleteEventMessage> context)
        {
            _logger.LogInformation("Company deleted event received.");

            var message = context.Message;

            try
            {                
                var result = await _companyService.DeleteAsync(message.Id);

                await _companyService.SaveChangesAsync();

                if (result)
                    _logger.LogInformation("Company ID {0} deleted", message.Id);
                else
                    _logger.LogWarning("Company ID {0} not found", message.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when trying to delete company ID {0}", message.Id);
            }
        }
    }
}
