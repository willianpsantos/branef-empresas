using Branef.Empresas.Data.Entities;
using Branef.Empresas.Domain.EventMessages;
using Branef.Empresas.Domain.Interfaces.Converters;
using Branef.Empresas.Domain.Models;

namespace Branef.Empresas.Domain.Converters
{
    public class CompanyDomainConverter : ICompanyDomainConverter
    {
        public InsertOrUpdateCompanyCommand ToInsertOrUpdateCommand(Company entity) =>
            new InsertOrUpdateCompanyCommand
            {
                Id = entity.Id,
                Name = entity.Name,
                Size = entity.Size
            };

        public InsertOrUpdateCompanyCommand ToInsertOrUpdateCommand(CompanyAddedOrUpdateEventMessage message) =>
            new InsertOrUpdateCompanyCommand
            {
                Id = message.Id,
                Name = message.Name,
                Size = message.Size
            };

        public CompanyModel ToModel(Company entity) =>
            new CompanyModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Size = entity.Size
            };

        public Company ToEntity(CompanyModel model) =>
            new Company
            {
                Id = model.Id,
                Name = model.Name,
                Size = model.Size
            };

        public Company ToEntity(InsertOrUpdateCompanyCommand model, Guid? id = null) =>
           new Company
           {
               Id = id ?? Guid.Empty,
               Name = model.Name,
               Size = model.Size
           };
    }
}
