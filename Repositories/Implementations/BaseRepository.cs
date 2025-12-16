using Artify.Api.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Artify.Api.Repositories.Implementations
{
    public abstract class BaseRepository
    {
        protected readonly ApplicationDbContext _context;

        protected BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        protected IQueryable<T> IncludeMultiple<T>(IQueryable<T> query, params Expression<Func<T, object>>[] includes) where T : class
        {
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return query;
        }

        protected async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}