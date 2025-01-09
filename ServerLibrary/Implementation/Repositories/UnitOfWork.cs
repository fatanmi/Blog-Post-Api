using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Implementation.Contract;
using ServerLibrary.Model.Entities;

namespace ServerLibrary.Implementation.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDBContext _Context;
        private readonly IGenericRepository<Post> _Posts;
        private readonly IGenericRepository<Comment> _Comments;

        public UnitOfWork(AppDBContext context)
        {
            _Context = context;
        }
        public IGenericRepository<Post> Posts => _Posts ?? new GenericRepository<Post>(_Context);

        public IGenericRepository<Comment> Comments => _Comments ?? new GenericRepository<Comment>(_Context);

        public void Dispose()
        {
            _Context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await _Context.SaveChangesAsync();
        }
    }
}
