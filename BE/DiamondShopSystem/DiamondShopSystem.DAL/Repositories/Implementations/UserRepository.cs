using System;
using System.Threading.Tasks;
using DiamondShopSystem.BLL.Application.Interfaces;
using DiamondShopSystem.BLL.Domain.Entities;
using DiamondShopSystem.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace DiamondShopSystem.DAL.Repositories.Implementations
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DiamondShopDbContext context) : base(context) { }

        public async Task<User?> GetByIdWithRoleAsync(Guid id)
        {
            return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);
        }
    }
} 