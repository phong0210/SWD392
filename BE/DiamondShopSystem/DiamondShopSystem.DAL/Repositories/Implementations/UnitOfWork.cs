using DiamondShopSystem.BLL.Application.Interfaces;
using DiamondShopSystem.BLL.Domain.Entities;
using DiamondShopSystem.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Concurrent;

namespace DiamondShopSystem.DAL.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DiamondShopDbContext _context;
        private readonly ConcurrentDictionary<Type, object> _repositories;

        public IRepository<Product> Products { get; private set; }
        public IRepository<Category> Categories { get; private set; }
        public IRepository<Order> Orders { get; private set; }
        public IUserRepository Users { get; private set; }
        public IRepository<Role> Roles { get; private set; }
        public IRepository<CustomerProfile> CustomerProfiles { get; private set; }
        public IRepository<VipStatus> VipStatuses { get; private set; }
        public IRepository<Promotion> Promotions { get; private set; }
        public IRepository<Delivery> Deliveries { get; private set; }

        public UnitOfWork(DiamondShopDbContext context)
        {
            _context = context;
            _repositories = new ConcurrentDictionary<Type, object>();

            Products = new ProductRepository(_context);
            Categories = new GenericRepository<Category>(_context);
            Orders = new GenericRepository<Order>(_context);
            Users = new UserRepository(_context);
            Roles = new GenericRepository<Role>(_context);
            CustomerProfiles = new GenericRepository<CustomerProfile>(_context);
            VipStatuses = new GenericRepository<VipStatus>(_context);
            Promotions = new GenericRepository<Promotion>(_context);
            Deliveries = new GenericRepository<Delivery>(_context);
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            _repositories.Clear();
            GC.SuppressFinalize(this);
        }
    }
}