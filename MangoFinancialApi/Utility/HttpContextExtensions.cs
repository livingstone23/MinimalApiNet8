using Microsoft.EntityFrameworkCore;

namespace MangoFinancialApi.Utility;



/// <summary>
/// Create the static class to extend the HttpContext
/// </summary>
public static class HttpContextExtensions
{


    /// <summary>
    /// Generic method to insert pagination parameters in the response
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="queryable"></param>
    /// <param name="recordsPerPage"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async static Task InsertPaginationParametersInResponse<T>(this HttpContext httpContext, IQueryable<T> queryable)
    {
        if (httpContext == null) { throw new ArgumentNullException(nameof(httpContext)); }

        double count = await queryable.CountAsync();
        httpContext.Response.Headers.Append("totalAmountPages", count.ToString());
        

    }

}
