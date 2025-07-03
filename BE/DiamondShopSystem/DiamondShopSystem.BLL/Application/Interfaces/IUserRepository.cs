using System;
using System.Threading.Tasks;
using DiamondShopSystem.BLL.Domain.Entities;

namespace DiamondShopSystem.BLL.Application.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByIdWithRoleAsync(Guid id);
    }
} 