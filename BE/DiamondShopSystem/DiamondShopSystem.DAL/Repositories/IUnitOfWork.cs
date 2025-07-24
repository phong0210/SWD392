using DiamondShopSystem.DAL.Repositories.Contracts;
using System;
using System.Threading.Tasks;

namespace DiamondShopSystem.DAL.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IOrderRepository OrderRepository { get; }
        IGenericRepository<T> Repository<T>() where T : class;
        Task<int> SaveChangesAsync();

    }
} 