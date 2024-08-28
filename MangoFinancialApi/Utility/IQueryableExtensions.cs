using MangoFinancialApi.Dto;



namespace MangoFinancialApi.Utility;



/// <summary>
/// Extension methods for IQueryable for pagination
/// </summary>
public static class IQueryableExtensions
{

    public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationDTO pagination)
    {
        return queryable
                    .Skip((pagination.Page - 1) * pagination.RecordsPerPage)
                    .Take(pagination.RecordsPerPage);
    }

}
