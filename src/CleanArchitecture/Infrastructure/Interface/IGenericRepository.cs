using System.Linq.Expressions;
using CleanArchitecture.Application.Common.Models;

namespace CleanArchitecture.Infrastructure.Interface;

public interface IGenericRepository<T> where T : BaseModel
{
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    Task<bool> AnyAsync(Expression<Func<T, bool>> filter);
    Task<bool> AnyAsync();
    Task<int> CountAsync(Expression<Func<T, bool>> filter);
    Task<int> CountAsync();
    Task<T> GetByIdAsync(object id);
    Task<Pagination<T>> ToPagination(int pageIndex, int pageSize, Expression<Func<T, object>>? orderBy = null, bool ascending = true);
    Task<Pagination<T>> GetAsync(Expression<Func<T, bool>> filter, int pageIndex = 0, int pageSize = 10, Expression<Func<T, object>>? orderBy = null, bool ascending = true);
    Task<Pagination<T>> GetAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IQueryable<T>>? include = null, int pageIndex = 0, int pageSize = 10);
    Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter);
    Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter,
   Expression<Func<T, object>> sort, bool ascending = true);
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);
    void Delete(T entity);
    void DeleteRange(IEnumerable<T> entities);
    Task Delete(object id);
}