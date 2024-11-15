using System.Linq.Expressions;

namespace Branef.Empresas.Domain.Interfaces.Base
{
    public interface IQueryToExpressionAdapter<TQuery, TEntity>
        where TEntity : class
        where TQuery : class
    {
        Expression<Func<TEntity, bool>>? ToExpression(TQuery? query);
    }
}
