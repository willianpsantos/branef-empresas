using Branef.Empresas.DB;
using Microsoft.EntityFrameworkCore;

namespace Branef.Empresas.Repositories
{
    public class CompanyReadRepository : CompanyRepository<BranefReadDbContext>
    {
        public CompanyReadRepository(BranefReadDbContext context) : base(context) { }

        public override async ValueTask<bool> DeleteAsync(Guid id)
        {
            var affected = await _context
               .Companies
               .FirstOrDefaultAsync(_ => _.Id == id);

            if (affected is null)
                return false;

            affected.DeletedAt = DateTime.UtcNow;
            affected.IsDeleted = true;

            return true;
        }
    }
}
