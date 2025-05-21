namespace API.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey>
        where TEntity : class
    {
        private readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(TEntity entity) => _context.Set<TEntity>().Add(entity);

        public void Delete(TEntity entity) => _context.Set<TEntity>().Remove(entity);

        public void DeleteRange(IEnumerable<TEntity> entities) => _context.Set<TEntity>().RemoveRange(entities);

        public void Update(TEntity entity) => _context.Set<TEntity>().Update(entity);

        public async Task<IEnumerable<TEntity>> GetAllAsync()
            => await _context.Set<TEntity>().ToListAsync();

        public async Task<TEntity?> GetAsync(TKey key)
            => await _context.Set<TEntity>().FindAsync(key);

        public async Task<TEntity?> GetAsync(ISpecifications<TEntity> specifications)
            => await SpecificationsEvaluator.CreateQuery(_context.Set<TEntity>(), specifications)
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity> specifications)
            => await SpecificationsEvaluator.CreateQuery(_context.Set<TEntity>(), specifications)
                .ToListAsync();

        public async Task<int> CountAsync(ISpecifications<TEntity> specifications)
            => await SpecificationsEvaluator.CreateQuery(_context.Set<TEntity>(), specifications)
                .CountAsync();

        public async Task<IEnumerable<TEntity>> FindAllWithcraiteriaAsync(Expression<Func<TEntity, bool>> craiteria)
        {
            IQueryable<TEntity> Query = _context.Set<TEntity>();
            return await Query.Where(craiteria).ToListAsync();
        }    
         public async Task<TEntity> FindWithcraiteriaAsync(Expression<Func<TEntity, bool>> craiteria)
        {
            IQueryable<TEntity> Query = _context.Set<TEntity>();
            return await Query.Where(craiteria).SingleOrDefaultAsync();
        }     
    }
}
