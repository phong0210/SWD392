using DiamondShopSystem.DAL.Entities;
using DiamondShopSystem.DAL.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondShopSystem.DAL.Repositories
{
    public class DeliveryRepository : GenericRepository<Delivery>, IDeliveryRepository
    {
        public DeliveryRepository(AppDbContext context) : base(context)
        {
        }
    }
}