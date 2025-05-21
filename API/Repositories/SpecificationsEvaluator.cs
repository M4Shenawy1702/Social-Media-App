namespace API.Repositories
{
internal static class SpecificationsEvaluator
{
    public static IQueryable<T> CreateQuery<T>(IQueryable<T> inputQuery, ISpecifications<T> specifications)
        where T : class
    {

        var query = inputQuery;
        if (specifications.Criteria is not null)
            query = query.Where(specifications.Criteria);

        query = specifications.Includes.
            Aggregate(query,
            (currentQuery, expression) => currentQuery.Include(expression));

        if (specifications.OrderBy is not null)
            query = query.OrderBy(specifications.OrderBy);
        else if (specifications.OrderByDesc is not null)
            query = query.OrderByDescending(specifications.OrderByDesc);

        if (specifications.IsPaginated)
            query = query.Skip(specifications.Skip).Take(specifications.Take);
        return query;
    }
}
}