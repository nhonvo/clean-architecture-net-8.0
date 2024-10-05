using System.Linq.Expressions;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Repositories;

public class GenericRepository<T>(ApplicationDbContext context) : IGenericRepository<T> where T : BaseModel
{
    protected DbSet<T> _dbSet = context.Set<T>();

    public async Task AddAsync(T entity)
        => await _dbSet.AddAsync(entity);

    public async Task AddRangeAsync(IEnumerable<T> entities)
        => await _dbSet.AddRangeAsync(entities);

    #region  Read

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
        => await _dbSet.AnyAsync(filter);

    public async Task<bool> AnyAsync()
        => await _dbSet.AnyAsync();

    public async Task<int> CountAsync(Expression<Func<T, bool>> filter)
    {
        return filter == null ? await _dbSet.CountAsync() : await _dbSet.CountAsync(filter);
    }

    public async Task<int> CountAsync()
        => await _dbSet.CountAsync();

    public async Task<T> GetByIdAsync(object id)
        => await _dbSet.FindAsync(id);

    public async Task<Pagination<T>> ToPagination(
        int pageIndex,
        int pageSize,
        Expression<Func<T, object>>? orderBy = null,
        bool ascending = true)
    {
        var itemCount = await _dbSet.CountAsync();

        IQueryable<T> query = _dbSet.AsNoTracking();

        orderBy ??= x => EF.Property<object>(x, "Id");

        query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

        var items = await query
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var result = new Pagination<T>()
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalItemsCount = itemCount,
            Items = items,
        };

        return result;
    }


    public async Task<Pagination<T>> GetAsync(
        Expression<Func<T, bool>> filter,
        int pageIndex = 0,
        int pageSize = 10,
        Expression<Func<T, object>>? orderBy = null,
        bool ascending = true)
    {
        var itemCount = await _dbSet.CountAsync();

        IQueryable<T> query = _dbSet.AsNoTracking().Where(filter);
        orderBy ??= x => EF.Property<object>(x, "Id");
        query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

        var items = await query
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var result = new Pagination<T>()
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalItemsCount = itemCount,
            Items = items,
        };

        return result;
    }

    public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter)
        => await _dbSet.IgnoreQueryFilters()
                        .AsNoTracking()
                        .FirstOrDefaultAsync(filter);

    #endregion
    #region Update & delete

    public void Update(T entity)
        => _dbSet.Update(entity);

    public void UpdateRange(IEnumerable<T> entities)
        => _dbSet.UpdateRange(entities);

    public void Delete(T entity)
        => _dbSet.Remove(entity);

    public void DeleteRange(IEnumerable<T> entities)
        => _dbSet.RemoveRange(entities);

    public async Task Delete(object id)
    {
        T entity = await GetByIdAsync(id);
        Delete(entity);
    }
    #endregion
}
