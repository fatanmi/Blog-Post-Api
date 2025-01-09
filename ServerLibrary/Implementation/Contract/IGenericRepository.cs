using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Implementation.Contract
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetAsync(Expression<Func<T, bool>> expression = null, List<string> includes = null);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, List<string> includes = null);
        void DeleteAsync(int id);
        void UpdateAsync(T entity);
        Task<T> Insert(T entity);
    }
}
