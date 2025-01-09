using ServerLibrary.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Implementation.Contract
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Post> Posts { get; }
        IGenericRepository<Comment> Comments { get; }
        Task Save();
    }
}

