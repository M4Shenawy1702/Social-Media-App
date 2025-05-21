using System;


namespace API.Contracts
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : class
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);
        Task<TEntity?> GetAsync(TKey key);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetAsync(ISpecifications<TEntity> specifications);
        Task<int> CountAsync(ISpecifications<TEntity> specifications);
        Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity> specifications);
        Task<IEnumerable<TEntity>> FindAllWithcraiteriaAsync(Expression<Func<TEntity, bool>> craiteria);
        Task<TEntity> FindWithcraiteriaAsync(Expression<Func<TEntity, bool>> craiteria);
    }
}