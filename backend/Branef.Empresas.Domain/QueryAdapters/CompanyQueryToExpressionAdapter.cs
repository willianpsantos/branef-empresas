using Branef.Empresas.Data.Entities;
using Branef.Empresas.Domain.Interfaces.Base;
using Branef.Empresas.Domain.Queries;
using System.Linq.Expressions;

namespace Branef.Empresas.Domain.QueryAdapters
{
    public class CompanyQueryToExpressionAdapter : IQueryToExpressionAdapter<CompanyQuery, Company>
    {
        public Expression<Func<Company, bool>>? ToExpression(CompanyQuery? query)
        {
            if (query is null)
                return null;

            if (query.Id.HasValue && query.Id != Guid.Empty)
                return _ => _.Id == query.Id;

            Expression<Func<Company, bool>> expr =
                _ => (query.Id == null || _.Id == query.Id) &&
                     (string.IsNullOrEmpty( query.Name ) || _.Name.Contains(query.Name)) &&
                     (query.Size == null || _.Size == query.Size) &&
                     _.IsDeleted == false;

            return expr;
        }
    }
}
