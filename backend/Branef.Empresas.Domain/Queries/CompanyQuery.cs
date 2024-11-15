using Branef.Empresas.Data.Enums;

namespace Branef.Empresas.Domain.Queries
{
    public class CompanyQuery
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public CompanySize? Size { get; set; }
    }
}
