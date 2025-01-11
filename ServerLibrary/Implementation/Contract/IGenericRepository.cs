using ServerLibrary.Implementation.Repositories;
using System.Linq.Expressions;
using X.PagedList;


namespace ServerLibrary.Implementation.Contract
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetAsync(Expression<Func<T, bool>> expression = null, List<string> includes = null);
        Task<IPagedList<T>> GetAllAsync(RequestParams requestParams = null, Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, List<string> includes = null);

        Task DeleteAsync(int id);
        void UpdateAsync(T entity);
        Task<T> Insert(T entity);
    }
}
