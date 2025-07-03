using System;
using System.Threading.Tasks;

namespace DiamondShopSystem.DAL.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : class;
        Task<int> SaveChangesAsync();
    }
} 