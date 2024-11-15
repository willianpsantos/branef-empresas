using Branef.Empresas.DB;
using Branef.Empresas.Domain.EventMessages;
using Branef.Empresas.Domain.Interfaces.Converters;
using Branef.Empresas.Domain.Interfaces.Services;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Branef.Empresas.Events
{
    public class CompanyInsertedOrUpdatedConsumer : IConsumer<CompanyAddedOrUpdateEventMessage>
    {
        private readonly ILogger<CompanyInsertedOrUpdatedConsumer> _logger;
        private readonly ICompanyDomainConverter _companyDomainConverter;
        private readonly ICompanyService<BranefReadDbContext> _companyService;

        public CompanyInsertedOrUpdatedConsumer(
            ICompanyService<BranefReadDbContext> companyService,
            ICompanyDomainConverter companyDomainConverter,
            ILogger<CompanyInsertedOrUpdatedConsumer> logger
        )
        {
            _logger = logger;
            _companyService = companyService;
            _companyDomainConverter = companyDomainConverter;
        }

        public async Task Consume(ConsumeContext<CompanyAddedOrUpdateEventMessage> context)
        {
            _logger.LogInformation("Company inserted or update event received.");

            var message = context.Message;

            try
            {
                var model = _companyDomainConverter.ToInsertOrUpdateCommand(message);
                var existent = await _companyService.GetByIdAsync(message.Id!.Value);

                if (existent is null)
                    await _companyService.InsertAsync(model);
                else
                    _companyService.Update(message.Id!.Value, model);

                var affected = await _companyService.SaveChangesAsync();

                if (affected > 0)
                    _logger.LogInformation("Company ID {0} updated", message.Id);
                else
                    _logger.LogWarning("Company not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when trying to insert or update company ID {0}", message.Id);
            }
        }
    }
}
