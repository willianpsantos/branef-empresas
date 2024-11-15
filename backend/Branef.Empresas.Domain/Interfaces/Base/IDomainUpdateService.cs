namespace Branef.Empresas.Domain.Interfaces.Base
{
    public interface IDomainUpdateService<TModel, TReturnModel> 
        where TModel : class
        where TReturnModel : class
    {
        TReturnModel Update(Guid id, TModel model);
    }
}
