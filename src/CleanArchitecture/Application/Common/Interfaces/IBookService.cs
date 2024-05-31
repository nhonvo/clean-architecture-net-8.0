using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Common.Models.Book;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IBookService
{
    Task<Pagination<Book>> Get(int pageIndex, int pageSize);
    Task<Book> Get(int id);
    Task Add(BookDTO request, CancellationToken token);
    Task Update(Book request, CancellationToken token);
    Task Delete(int id, CancellationToken token);
}
