using System.Linq.Expressions;
using System.Threading;
using AutoMapper;
using CleanArchitecture.Application;
using CleanArchitecture.Application.Common.Models.Book;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Interface;
using Moq;

namespace CleanArchitecture.Unittest.Application;

public class BookServiceTest
{
    private readonly BookService _bookService;
    private readonly CancellationToken _cancellationToken;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IBookRepository> _bookRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly Book _book = new Book { Id = 1, Title = "a", Description = "abc" };
    private readonly BookDTO _bookDto = new BookDTO();


    public BookServiceTest()
    {
        // Arrange
        _cancellationToken = new CancellationToken();
        _unitOfWork = new Mock<IUnitOfWork>();
        _bookRepository = new Mock<IBookRepository>();
        _mapper = new Mock<IMapper>();
        _mapper.Setup(x => x.Map<Book>(_bookDto)).Returns(_book);
        _bookService = new BookService(_unitOfWork.Object, _mapper.Object);
    }

    [Fact]
    public async Task Get_Returns_PaginationOfBooks()
    {
        // Arrange
        var pageIndex = 1;
        var pageSize = 10;
        _unitOfWork.Setup(uow => uow.BookRepository).Returns(_bookRepository.Object);

        // Act
        var result = await _bookService.Get(pageIndex, pageSize);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Get_ById_Returns_Book()
    {
        // Arrange
        var bookId = 1;
        var expectedBook = new Book { Id = bookId, Title = "Test Book" };
        _bookRepository.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<Book, bool>>>()))
            .ReturnsAsync(expectedBook);
        _unitOfWork.Setup(uow => uow.BookRepository).Returns(_bookRepository.Object);

        // Act
        var result = await _bookService.Get(bookId);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Book>(result);
        Assert.Equal(expectedBook.Id, result.Id);
        Assert.Equal(expectedBook.Title, result.Title);
    }

    [Fact]
    public async Task Add_ShouldAddBookAndReturnId()
    {
        // Act
        var result = await _bookService.Add(_bookDto, _cancellationToken);

        // Assert
        Assert.Equal(_book.Id, result);
    }

    [Fact]
    public async Task Update_ShouldUpdateBookAndReturnBookDto()
    {
        // Arrange
        _unitOfWork.Setup(uow => uow.BookRepository.FirstOrDefaultAsync(It.IsAny<Expression<Func<Book, bool>>>()))
                     .ReturnsAsync(_book);
        
        // Act
        var result = await _bookService.Update(_book, _cancellationToken);

        // Assert
        Assert.Equal(_book.Id, result);
    }

    [Fact]
    public async Task Delete_ShouldDeleteBookAndReturnBookId()
    {
        // Arrange

        var bookId = 1;

        var book = new Book { Id = bookId };
        _unitOfWork.Setup(uow => uow.BookRepository.FirstOrDefaultAsync(It.IsAny<Expression<Func<Book, bool>>>()))
                     .ReturnsAsync(book);

        // Act
        var result = await _bookService.Delete(bookId, _cancellationToken);

        // Assert
        Assert.Equal(book.Id, result);
    }
}
