
using Domain.Wrapper;
using Microsoft.EntityFrameworkCore;

namespace System.Linq;
public static class QueryableExtensions
{
    public static async Task<ListResult<T>> ToListResultAsync<T>(this IQueryable<T> source, CancellationToken cancellationToken = default)
    {
        var items = await source.ToListAsync(cancellationToken);
        return ListResult<T>.Success(items);
    }
}
