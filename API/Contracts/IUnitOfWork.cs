namespace API.Contracts
{
    public interface IUnitOfWork
    {
           Task<int> SaveChangesAsync();

            IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
                where TEntity : class;
    }
}