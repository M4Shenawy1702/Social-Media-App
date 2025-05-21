using System.Linq.Expressions;
using API.Contracts;

namespace API.Specifications;
public abstract class BaseSpecifications<T>(Expression<Func<T, bool>>? criteria)
    : ISpecifications<T>
    where T : class
{
    public Expression<Func<T, bool>> Criteria { get; } = criteria!;
    public List<Expression<Func<T, object>>> Includes { get; } = [];
    public Expression<Func<T, object>> OrderBy { get; private set; }
    public Expression<Func<T, object>> OrderByDesc { get; private set; }

    public int Skip { get; private set; }
    public int Take { get; private set; }
    public bool IsPaginated { get; private set; }
    protected void ApplyPagination(int pageSize, int pageIndex)
    {
        IsPaginated = true;
        Take = pageSize;
        Skip = (pageIndex - 1) * pageSize;
    }
    protected void AddInclude(Expression<Func<T, object>> expression) => Includes.Add(expression);
    protected void AddOrderBy(Expression<Func<T, object>> expression) => OrderBy = expression;
    protected void AddOrderByDesc(Expression<Func<T, object>> expression) => OrderByDesc = expression;
}
