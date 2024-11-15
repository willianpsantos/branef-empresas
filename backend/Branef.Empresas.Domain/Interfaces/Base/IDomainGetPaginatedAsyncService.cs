namespace Branef.Empresas.Domain.Interfaces.Base
{
    public interface IDomainGetPaginatedAsyncService<TQuery, TModel>
        where TModel : class
        where TQuery : class
    {
        ValueTask<IPaginatedResponse<TModel>> GetPaginatedAsync(int page, int pageSize, TQuery? query = default);
    }
}
