using DiamondShopSystem.DAL.Entities;
using DiamondShopSystem.DAL.Repositories.Contracts;

namespace DiamondShopSystem.DAL.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }
    }
}