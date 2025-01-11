using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Implementation.Contract;
using System.Linq.Expressions;
using X.PagedList;
using X.PagedList.Extensions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ServerLibrary.Implementation.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDBContext _dbContext;
        private readonly DbSet<T> _db;

        public GenericRepository(AppDBContext _Context)
        {
            _dbContext = _Context;
            _db = _dbContext.Set<T>();
        }
        public async Task DeleteAsync(int id)
        {

            T Entity = await _db.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);

            if (Entity is not null)
            {
                _db.Remove(Entity);
                await _dbContext.SaveChangesAsync();
            }

        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> expression = null, List<string> includes = null)
        {
            IQueryable<T> Query = _db;

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    Query = Query.Include(include);
                }
            }
            if (expression != null)
            {
                return await Query.AsNoTracking().FirstOrDefaultAsync(expression);
            }
            return await Query.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<IPagedList<T>> GetAllAsync(RequestParams requestParams = null, Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, List<string> includes = null)
        {
            IQueryable<T> Query = _db;

            if (expression != null)
            {
                Query = Query.Where(expression);
            }

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    Query = Query.Include(include);
                }
            }
            if (orderby != null)
            {
                Query = orderby(Query);
            }

            int TotalCount = await Query.CountAsync();
            int PageNumber = requestParams?.PageNumber ?? 1;
            int PageSize = requestParams?.PageSize ?? requestParams.MaxPageSize;
            PageSize = PageSize > 0 ? PageSize : requestParams.MaxPageSize;

            List<T> Items = await Query.Skip((PageNumber - 1) * PageSize)
                                   .Take(PageSize)
                                   .AsNoTracking()
                                   .ToListAsync();
            return new StaticPagedList<T>(Items, PageNumber, PageSize, TotalCount);


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
