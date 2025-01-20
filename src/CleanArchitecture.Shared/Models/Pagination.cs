using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Shared.Models;

public class Pagination<T>(List<T> items, int count, int pageIndex, int pageSize)
{
    public int CurrentPage { get; private set; } = pageIndex;
    public int TotalPages { get; private set; } = (int)Math.Ceiling(count / (double)pageSize);
    public int PageSize { get; private set; } = pageSize;
    public int TotalCount { get; private set; } = count;
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
    public List<T>? Items { get; private set; } = items;

    public static async Task<Pagination<T>> ToPagedList(IQueryable<T> source, int pageIndex, int pageSize)
    {
        pageIndex = pageIndex <= 0 ? 1 : pageIndex;
        pageSize = pageSize <= 0 ? 10 : pageSize;

        var count = source.Count();
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        return new Pagination<T>(items, count, pageIndex, pageSize);
    }
}
