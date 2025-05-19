namespace UserManagementSystem.Data.Extensions;

public static class PaginationExtensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int page, int pageSize)
    {
        if (pageSize <= 0 || pageSize > 100) pageSize = 10;
        if (page <= 0) page = 1;
        
        return query.Skip((page - 1) * pageSize).Take(pageSize);
    }
}