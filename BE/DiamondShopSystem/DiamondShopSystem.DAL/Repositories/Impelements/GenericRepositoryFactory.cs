using DiamondShopSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace DiamondShopSystem.DAL.Repositories.Impelements
{
    public class GenericRepositoryFactory<TContext> : IGenericRepositoryFactory where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly ConcurrentDictionary<Type, object> _repositories = new();

        public GenericRepositoryFactory(TContext context)
        {
            _context = context;
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                var repo = new GenericRepository<TEntity>(_context);
                _repositories[type] = repo;
            }
            return (IGenericRepository<TEntity>)_repositories[type];
        }
    }
}
