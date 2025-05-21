namespace API.Contracts
{

public interface ISpecifications<T> where T : class
{
    Expression<Func<T, bool>> Criteria { get; }
    List<Expression<Func<T, object>>> Includes { get; }
    Expression<Func<T, object>> OrderBy { get; }
    Expression<Func<T, object>> OrderByDesc { get; }
    int Skip { get; }
    int Take { get; }
    bool IsPaginated { get; }

}
}