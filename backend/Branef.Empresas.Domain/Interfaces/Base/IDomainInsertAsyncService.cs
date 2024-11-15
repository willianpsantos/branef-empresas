namespace Branef.Empresas.Domain.Interfaces.Base
{
    public interface IDomainInsertAsyncService<TModel, TReturnModel> 
        where TModel : class 
        where TReturnModel : class
    {
        ValueTask<TReturnModel> InsertAsync(TModel model);
    }
}
