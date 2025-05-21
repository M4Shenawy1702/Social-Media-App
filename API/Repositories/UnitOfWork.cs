

namespace API.Repositories
{
    public class UnitOfWork(ApplicationDbContext _context)
     : IUnitOfWork
    {
        private readonly Dictionary<string, object> _repositories = [];
         public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : class
        {
            var typeName = typeof(TEntity).Name;
            if (_repositories.ContainsKey(typeName))
                return (IGenericRepository<TEntity, TKey>)_repositories[typeName];
            var repo = new GenericRepository<TEntity, TKey>(_context);
            _repositories.Add(typeName, repo);
            return repo;
        }
    }
}