using AutoMapper;
using Clean.Architecture.Application.Common.Interfaces;
using Clean.Architecture.Application.Common.Models;
using Clean.Architecture.Application.Common.Models.Book;

namespace Clean.Architecture.Application.Services
{
    public class BookService(IUnitOfWork unitOfWork, IMapper mapper, ILoggerFactory logger) : IBookService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger _logger = logger.CreateLogger<BookService>();

        public async Task<Pagination<Book>> Get(int pageIndex, int pageSize)
        {
            var books = await _unitOfWork.BookRepository.ToPagination(pageIndex, pageSize);
            return books;
        }

        public async Task<Book> Get(int id)
        {
            var book = await _unitOfWork.BookRepository.FirstOrDefaultAsync(x => x.Id == id);
            return book;
        }

        public async Task<int> Add(BookDTO request, CancellationToken token)
        {
            var book = _mapper.Map<Book>(request);
            await _unitOfWork.ExecuteTransactionAsync(async () => await _unitOfWork.BookRepository.AddAsync(book), token);
            return book.Id;
        }
        public async Task<BookDTO> Update(Book request, CancellationToken token)
        {
            var book = await _unitOfWork.BookRepository.FirstOrDefaultAsync(x => x.Id == request.Id);
            book = _mapper.Map<Book>(request);
            await _unitOfWork.ExecuteTransactionAsync(() => _unitOfWork.BookRepository.Update(book), token);
            var result = _mapper.Map<BookDTO>(book);
            return result;
        }
        public async Task<int> Delete(int id, CancellationToken token)
        {

            var book = await _unitOfWork.BookRepository.FirstOrDefaultAsync(x => x.Id == id);
            await _unitOfWork.ExecuteTransactionAsync(() => _unitOfWork.BookRepository.Delete(book), token);
            return book.Id;
        }
    }
}
