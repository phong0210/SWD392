using DiamondShopSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiamondShopSystem.DAL.Repositories.Impelements
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<TEntity?> GetByIdAsync(object id) => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<TEntity>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task AddAsync(TEntity entity) => await _dbSet.AddAsync(entity);

        public void Update(TEntity entity) => _dbSet.Update(entity);

        public void Remove(TEntity entity) => _dbSet.Remove(entity);
    }
}
