using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Implementation.Contract;
using System.Linq.Expressions;
namespace ServerLibrary.Implementation.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDBContext _dbContext;
        private readonly DbSet<T> _db;

        public GenericRepository(AppDBContext appDBContext)
        {
            _dbContext = appDBContext;
            _db = _dbContext.Set<T>();
        }
        public async void DeleteAsync(int id)
        {
            T Entity = await _db.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);

            if (Entity is not null) { _db.Remove(Entity); }
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> expression = null, List<string> includes = null)
        {
            IQueryable<T> query = _db;

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.AsNoTracking().FirstOrDefaultAsync(expression);
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, List<string> includes = null)
        {
            IQueryable<T> query = _db;

            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.AsNoTracking().ToListAsync();
        }



        public void UpdateAsync(T Entity)
        {
            _db.Attach(Entity);
            _dbContext.Entry(Entity).State = EntityState.Modified;
        }

        public async Task<T> Insert(T Entity)
        {
            var result = await _db.AddAsync(Entity);

            await _dbContext.SaveChangesAsync();

            return result.Entity;

        }

    }
}
