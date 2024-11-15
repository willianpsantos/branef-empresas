using Branef.Empresas.Data.Base;
using Branef.Empresas.Data.Enums;

namespace Branef.Empresas.Data.Entities
{
    public class Company : Entity
    {
        public string Name { get; set; }
        public CompanySize Size { get; set; }
    }
}
