using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Shared.Models.Book;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IBookService
{
    Task<Pagination<BookDTO>> Get(int pageIndex, int pageSize);
    Task<BookDTO> Get(int id);
    Task<BookDTO> Add(AddBookRequest request, CancellationToken token);
    Task<BookDTO> Update(UpdateBookRequest request, CancellationToken token);
    Task<BookDTO> Delete(int id, CancellationToken token);
}
