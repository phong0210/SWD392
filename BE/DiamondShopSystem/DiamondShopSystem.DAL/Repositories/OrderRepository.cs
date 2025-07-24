using DiamondShopSystem.DAL.Entities;
using DiamondShopSystem.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DiamondShopSystem.DAL.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId)
        {
            return await _context.Set<Order>().Where(o => o.UserId == userId).ToListAsync();
        }
    }
}