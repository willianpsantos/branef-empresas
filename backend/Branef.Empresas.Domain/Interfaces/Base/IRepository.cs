using System.Linq.Expressions;

namespace Branef.Empresas.Domain.Interfaces.Base
{
    public interface IRepository<TEntity, TDbContext> 
        where TEntity : class 
        where TDbContext : class
    {
        ValueTask<int> CountAsync(Expression<Func<TEntity, bool>>? query = default);

        IAsyncEnumerable<TEntity> GetAsync(Expression<Func<TEntity, bool>>? query = default, int page = 0, int pageSize = 0);
        ValueTask<TEntity?> GetByIdAsync(Guid id);

        ValueTask<TEntity> InsertAsync(TEntity entity);
        ValueTask<TEntity> InsertAndSaveChangesAsync(TEntity entity);
        TEntity Update(TEntity entity);
        ValueTask<TEntity> UpdateAndSaveChangesAsync(TEntity entity);
        ValueTask<bool> DeleteAsync(Guid id);
        ValueTask<bool> DeleteAndSaveChangesAsync(Guid id);
        ValueTask<int> SaveChangesAsync();
    }
}
