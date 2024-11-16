using Branef.Empresas.Data.Entities;
using Branef.Empresas.DB;
using Branef.Empresas.Domain.Interfaces.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Branef.Empresas.Repositories
{
    public class CompanyRepository<TDbContext> : IRepository<Company, TDbContext> 
        where TDbContext : BranefDbContext<TDbContext>
    {
        protected readonly TDbContext _context;

        public CompanyRepository(TDbContext context) => _context = context;

        protected virtual IQueryable<Company> GetBaseQuery(Expression<Func<Company, bool>>? query = default)
        {
            IQueryable<Company> queryable =
                (query is null)
                    ? _context.Companies.Where(_ => !_.IsDeleted)
                    : _context.Companies.Where(query);

            return queryable;
        }

        public virtual async ValueTask<int> CountAsync(Expression<Func<Company, bool>>? query = default)
        {
            IQueryable<Company> queryable = GetBaseQuery(query);
            
            return await queryable.CountAsync();
        }

        public virtual async IAsyncEnumerable<Company> GetAsync(
            Expression<Func<Company, bool>>? query = null, 
            int page = 0, 
            int pageSize = 0
        )
        {
            IQueryable<Company>? queryable = GetBaseQuery(query);

            if (page > 0 && pageSize > 0)
            {
                queryable =
                    queryable
                        .OrderByDescending(_ => _.IncludedAt)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize);
            }

            await foreach (var item in queryable.AsNoTracking().AsAsyncEnumerable())
                yield return item;
        }

        public virtual async ValueTask<Company?> GetByIdAsync(Guid id) => await 
            _context
                .Companies
                .AsNoTracking()
                .FirstOrDefaultAsync(_ => _.Id == id && !_.IsDeleted);

        public virtual async ValueTask<Company> InsertAsync(Company entity)
        {
            if (entity.Id == Guid.Empty)
                entity.Id = Guid.NewGuid();

            entity.IncludedAt = DateTimeOffset.UtcNow;
            entity.IsDeleted = false;

            var entry = await _context.Companies.AddAsync(entity);

            return entry.Entity;
        }

        public virtual async ValueTask<Company> InsertAndSaveChangesAsync(Company entity)
        {
            var insertedEntity = await InsertAsync(entity);

            await _context.SaveChangesAsync();

            return insertedEntity;
        }

        public virtual Company Update(Company entity)
        {
            if (entity.Id == Guid.Empty)
                throw new InvalidOperationException("Entity has no ID");

            entity.UpdatedAt = DateTimeOffset.UtcNow;

            var trackedCompany =
               _context
                   .ChangeTracker
                   .Entries<Company>()?
                   .FirstOrDefault(_ => _.Entity?.Id == entity.Id);
           
            if (trackedCompany is null)
                _context.Entry(entity).State = EntityState.Modified;
            else
                trackedCompany.CurrentValues.SetValues(entity);

            return entity;
        }

        public virtual async ValueTask<Company> UpdateAndSaveChangesAsync(Company entity)
        {
            Update(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        public virtual async ValueTask<bool> DeleteAsync(Guid id)
        {
            var affected = await _context
                .Companies
                .Where(_ => _.Id == id)
                .ExecuteUpdateAsync(
                    _ => _.SetProperty(_ => _.IsDeleted, true)
                          .SetProperty(_ => _.DeletedAt, DateTimeOffset.UtcNow)
                );

            return affected > 0;
        }

        public virtual async ValueTask<bool> DeleteAndSaveChangesAsync(Guid id)
        {
            var deleted = await DeleteAsync(id);

            if (deleted)
                await _context.SaveChangesAsync();

            return deleted;
        }

        public virtual async ValueTask<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
