using DiamondShopSystem.DAL.Entities;
using DiamondShopSystem.DAL.Repositories;

namespace DiamondShopSystem.DAL.Repositories
{
    public class VipRepository : GenericRepository<Vip>, IVipRepository
    {
        public VipRepository(AppDbContext context) : base(context)
        {
        }
    }
}
