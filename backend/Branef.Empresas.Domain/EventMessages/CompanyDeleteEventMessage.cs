namespace Branef.Empresas.Domain.EventMessages
{
    public class CompanyDeleteEventMessage
    {
        public Guid Id { get; set; }
        public DateTimeOffset DeletedAt { get; set; }
    }
}
