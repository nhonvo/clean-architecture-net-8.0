using Clean.Architecture.Application.Common.Models;
using Clean.Architecture.Application.Common.Models.Book;

namespace Clean.Architecture.Application.Common.Interfaces
{
    public interface IBookService
    {
        Task<Pagination<Book>> Get(int pageIndex, int pageSize);
        Task<Book> Get(int id);
        Task<int> Add(BookDTO request, CancellationToken token);
        Task<BookDTO> Update(Book request, CancellationToken token);
        Task<int> Delete(int id, CancellationToken token);
    }
}
