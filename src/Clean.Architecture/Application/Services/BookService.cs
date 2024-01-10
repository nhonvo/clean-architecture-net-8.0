using System.Text.Json;
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
            _logger.LogInformation("Request: " + JsonSerializer.Serialize(new { PageIndex = pageIndex, PageSize = pageSize }));
            var books = await _unitOfWork.BookRepository.ToPagination(pageIndex, pageSize);
            _logger.LogInformation("Response: " + JsonSerializer.Serialize(books));
            return books;
        }

        public async Task<Book> Get(int id)
        {
            _logger.LogInformation("Request: " + JsonSerializer.Serialize(id));
            var book = await _unitOfWork.BookRepository.FirstOrDefaultAsync(x => x.Id == id);
            _logger.LogInformation("Response: " + JsonSerializer.Serialize(book));
            return book;
        }

        public async Task<int> Add(BookDTO request)
        {
            _logger.LogInformation("Request: " + JsonSerializer.Serialize(request));

            var book = _mapper.Map<Book>(request);
            await _unitOfWork.ExecuteTransactionAsync(async () => await _unitOfWork.BookRepository.AddAsync(book));
            _logger.LogInformation("Response: " + JsonSerializer.Serialize(book.Id));
            return book.Id;
        }
        public async Task<BookDTO> Update(Book request)
        {
            _logger.LogInformation("Request: " + JsonSerializer.Serialize(request));
            var book = await _unitOfWork.BookRepository.FirstOrDefaultAsync(x => x.Id == request.Id);
            book = _mapper.Map<Book>(request);
            await _unitOfWork.ExecuteTransactionAsync(() => _unitOfWork.BookRepository.Update(book));
            var result = _mapper.Map<BookDTO>(book);

            _logger.LogInformation("Response: " + JsonSerializer.Serialize(book.Id));

            return result;
        }
        public async Task<int> Delete(int id)
        {
            _logger.LogInformation("Request: " + JsonSerializer.Serialize(id));

            var book = await _unitOfWork.BookRepository.FirstOrDefaultAsync(x => x.Id == id);
            await _unitOfWork.ExecuteTransactionAsync(() => _unitOfWork.BookRepository.Delete(book));

            _logger.LogInformation("Response: " + JsonSerializer.Serialize(book.Id));

            return book.Id;
        }
    }
}
