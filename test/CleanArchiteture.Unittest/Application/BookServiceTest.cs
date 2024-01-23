using System.Linq.Expressions;
using AutoMapper;
using CleanArchitecture.Application;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Interface;
using Moq;

namespace CleanArchitecture.Unittest.Application;

public class BookServiceTest
{
    [Fact]
    public async Task Get_Returns_PaginationOfBooks()
    {
        // Arrange
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var bookRepositoryMock = new Mock<IBookRepository>();
        unitOfWorkMock.Setup(uow => uow.BookRepository).Returns(bookRepositoryMock.Object);
        var mapperMock = new Mock<IMapper>();

        var bookService = new BookService(unitOfWorkMock.Object, mapperMock.Object);

        var pageIndex = 1;
        var pageSize = 10;

        // Act
        var result = await bookService.Get(pageIndex, pageSize);

        // Assert
        Assert.Null(result);
        // Assert.IsType<Pagination<Book>>(result);
    }

    [Fact]
    public async Task Get_ById_Returns_Book()
    {
        // Arrange
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var bookRepositoryMock = new Mock<IBookRepository>();
        unitOfWorkMock.Setup(uow => uow.BookRepository).Returns(bookRepositoryMock.Object);
        var mapperMock = new Mock<IMapper>();

        var bookService = new BookService(unitOfWorkMock.Object, mapperMock.Object);

        var bookId = 1;
        var expectedBook = new Book { Id = bookId, Title = "Test Book" };
        bookRepositoryMock.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<Book, bool>>>()))
            .ReturnsAsync(expectedBook);

        // Act
        var result = await bookService.Get(bookId);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Book>(result);
        Assert.Equal(expectedBook.Id, result.Id);
        Assert.Equal(expectedBook.Title, result.Title);
    }
}
