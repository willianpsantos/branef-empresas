namespace Branef.Empresas.Domain.Interfaces.Base
{
    public interface IDomainSaveChangesAsyncService
    {
        ValueTask<int> SaveChangesAsync();
    }
}
