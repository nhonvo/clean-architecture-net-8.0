using AutoMapper;
using CleanArchitecture.Application;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.Entities;
using Moq;

namespace CleanArchitecture.Unittest.Application.Services;

public class BookServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly BookService _bookService;

    public BookServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _bookService = new BookService(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Get_ById_ShouldReturnBook()
    {
        // Arrange
        int bookId = 1;
        var book = new Book { Id = bookId };
        _unitOfWorkMock.Setup(u => u.BookRepository.FirstOrDefaultAsync(b => b.Id == bookId))
                       .ReturnsAsync(book);

        // Act
        var result = await _bookService.Get(bookId);

        // Assert
        Assert.Equal(book, result);
    }
}

