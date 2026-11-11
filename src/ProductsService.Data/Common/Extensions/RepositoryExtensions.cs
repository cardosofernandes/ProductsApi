using Microsoft.EntityFrameworkCore;
using ProductsService.Data.Common.Models;

namespace ProductsService.Data.Common.Extensions;

internal static class RepositoryExtensions
{
    public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> query, PagedRequest page, CancellationToken ct)
    {
        var offset = checked((int)(((long)page.PageNumber - 1) * page.PageSize));
        var total = await query.CountAsync(ct);
        var items = await query
            .Skip(offset)
            .Take(page.PageSize)
            .ToListAsync(ct);

        return new PagedList<T>(items, page.PageNumber, page.PageSize, total);
    }
    
    public static IQueryable<TEntity> TrackChanges<TEntity>(this IQueryable<TEntity> source, bool trackingChanges = false)
        where TEntity : class
        => trackingChanges ? source : source.AsNoTracking();
}