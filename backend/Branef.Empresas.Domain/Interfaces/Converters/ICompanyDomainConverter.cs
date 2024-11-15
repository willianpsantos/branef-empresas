using Branef.Empresas.Data.Entities;
using Branef.Empresas.Domain.EventMessages;
using Branef.Empresas.Domain.Models;

namespace Branef.Empresas.Domain.Interfaces.Converters
{
    public interface ICompanyDomainConverter
    {
        InsertOrUpdateCompanyCommand ToInsertOrUpdateCommand(Company entity);
        InsertOrUpdateCompanyCommand ToInsertOrUpdateCommand(CompanyAddedOrUpdateEventMessage message);
        CompanyModel ToModel(Company entity);
        
        Company ToEntity(CompanyModel model);
        Company ToEntity(InsertOrUpdateCompanyCommand model, Guid? id = null);
    }
}
