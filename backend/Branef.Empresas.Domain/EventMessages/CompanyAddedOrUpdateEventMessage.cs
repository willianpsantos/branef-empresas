using Branef.Empresas.Data.Enums;

namespace Branef.Empresas.Domain.EventMessages
{
    public class CompanyAddedOrUpdateEventMessage
    {
        public Guid? Id { get; set; }
        public string Name {  get; set; }
        public CompanySize Size { get; set; }
        public DateTimeOffset IncludedOrUpdatedAt { get; set; }
    }
}
