using Branef.Empresas.Data.Entities;
using Branef.Empresas.DB;
using Branef.Empresas.Domain.Interfaces.Base;
using Branef.Empresas.Domain.Interfaces.Converters;
using Branef.Empresas.Domain.Interfaces.Services;
using Branef.Empresas.Domain.Models;
using Branef.Empresas.Domain.Queries;
using Branef.Empresas.Domain.Responses;

namespace Branef.Empresas.Services
{
    public class CompanyService<TDbContext> : ICompanyService<TDbContext> where TDbContext : BranefDbContext<TDbContext>
    {
        private readonly IRepository<Company, TDbContext> _repository;
        private readonly ICompanyDomainConverter _domainConverter;
        private readonly IQueryToExpressionAdapter<CompanyQuery, Company> _queryAdapter;
        

        public CompanyService(
            IRepository<Company, TDbContext> repository,
            ICompanyDomainConverter domainConverter,
            IQueryToExpressionAdapter<CompanyQuery, Company> queryAdapter
        ) 
        {
            _repository = repository;            
            _queryAdapter = queryAdapter;
            _domainConverter = domainConverter;
        }
        

        public async ValueTask<int> CountAsync(CompanyQuery? query = null)
        {
            var expression = _queryAdapter?.ToExpression(query);            
            return await _repository.CountAsync(expression);
        }

        public async ValueTask<IEnumerable<CompanyModel>> GetAsync(CompanyQuery? query = null)
        {
            var expression = _queryAdapter?.ToExpression(query);
            var list = new HashSet<CompanyModel>();

            await foreach (var entity in _repository.GetAsync(expression))
                list.Add(_domainConverter.ToModel(entity));

            return list;
        }

        public async ValueTask<IPaginatedResponse<CompanyModel>> GetPaginatedAsync(int page, int pageSize, CompanyQuery? query = null)
        {
            var expression = _queryAdapter?.ToExpression(query);
            var list = new HashSet<CompanyModel>();

            await foreach (var entity in _repository.GetAsync(expression, page, pageSize))
                list.Add(_domainConverter.ToModel(entity));

            var count = await _repository.CountAsync(expression);

            return new PaginatedResponse<CompanyModel>
            {
                Count = count,
                PageSize = pageSize,
                PageNumber = page,
                Data = list
            };
        }

        public async ValueTask<CompanyModel?> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity is null ? null : _domainConverter.ToModel(entity);
        }

        public async ValueTask<CompanyModel> InsertAsync(InsertOrUpdateCompanyCommand model)
        {
            var entity = _domainConverter.ToEntity(model, model.Id);
            var inserted = await _repository.InsertAsync(entity);            

            return _domainConverter.ToModel(inserted);
        }   

        public CompanyModel Update(Guid id, InsertOrUpdateCompanyCommand model)
        {
            var entity = _domainConverter.ToEntity(model, id);
            var updated = _repository.Update(entity);
            
            return _domainConverter.ToModel(updated);
        }

        public async ValueTask<bool> DeleteAsync(Guid id) => await _repository.DeleteAsync(id);

        public async ValueTask<int> SaveChangesAsync() => await _repository.SaveChangesAsync();

    }
}
