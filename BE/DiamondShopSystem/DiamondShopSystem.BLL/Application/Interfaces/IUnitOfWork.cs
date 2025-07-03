using System;
using System.Threading.Tasks;
using DiamondShopSystem.BLL.Domain.Entities;

namespace DiamondShopSystem.BLL.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Product> Products { get; }
        IRepository<Category> Categories { get; }
        IRepository<Order> Orders { get; }
        IUserRepository Users { get; }
        IRepository<Role> Roles { get; }
        IRepository<CustomerProfile> CustomerProfiles { get; }
        IRepository<VipStatus> VipStatuses { get; }
        IRepository<Promotion> Promotions { get; }
        IRepository<Delivery> Deliveries { get; }
        // Add other aggregate roots here
        Task<int> CommitAsync();
    }
}
