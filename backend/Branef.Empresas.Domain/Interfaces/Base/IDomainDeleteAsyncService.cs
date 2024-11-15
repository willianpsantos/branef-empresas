namespace Branef.Empresas.Domain.Interfaces.Base
{
    public interface IDomainDeleteAsyncService
    {
        ValueTask<bool> DeleteAsync(Guid id);
    }
}
