using Branef.Empresas.Data.Enums;
using MediatR;
using System.Text.Json.Serialization;

namespace Branef.Empresas.Domain.Models
{
    public record InsertOrUpdateCompanyCommand : IRequest<CompanyModel>
    {
        [JsonPropertyName("id")]
        public Guid? Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("size")]
        public CompanySize Size { get; set; }
    }
}
