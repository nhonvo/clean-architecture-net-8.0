using Clean.Architecture.Application.Common.Models;
using Clean.Architecture.Application.Common.Models.Book;

namespace Clean.Architecture.Application.Common.Interfaces
{
    public interface IBookService
    {
        Task<Pagination<Book>> Get(int pageIndex, int pageSize);
        Task<Book> Get(int id);
        Task<int> Add(BookDTO request);
        Task<BookDTO> Update(Book request);
        Task<int> Delete(int id);
    }
}
