using System.Linq.Expressions;
using AutoMapper;
using CleanArchitecture.Application;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Common.Models.Book;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CleanArchitecture.Unittest.Application.Services;

public class BookTest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
    private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
    private BookService _bookService;

    [Fact]
    public async Task BookService_GetById_ShouldReturnABook()
    {
        // Arrange
        int bookId = 1;
        var expectedResult = new Book
        {
            Id = 1,
            Title = "C# Programming",
            Description = "A comprehensive guide to C# programming.",
            Price = 29.99
        };
        _unitOfWorkMock.Setup(u => u.BookRepository.FirstOrDefaultAsync(b => b.Id == bookId, null))
                       .ReturnsAsync(expectedResult);

        _bookService = new BookService(_unitOfWorkMock.Object, _mapperMock.Object);


        // Act
        var actualResult = await _bookService.Get(bookId);

        // Assert
        Assert.Equal(expectedResult.Id, actualResult.Id);
        Assert.Equal(expectedResult.Title, actualResult.Title);
        Assert.Equal(expectedResult.Description, actualResult.Description);
        Assert.Equal(expectedResult.Price, actualResult.Price);
    }

    [Fact]
    public async Task BookService_Get_ShouldReturnBooks()
    {
        // Arrange
        var expectedBooks = new List<Book>
        {
            new Book
            {
                Id = 1,
                Title = "C# Programming",
                Description = "A comprehensive guide to C# programming.",
                Price = 29.99
            },
            new Book
            {
                Id = 2,
                Title = "ASP.NET Core Development",
                Description = "Learn how to build web applications using ASP.NET Core.",
                Price = 35.50
            }
        };

        var expectedResult = new Pagination<Book>
        {
            TotalItemsCount = expectedBooks.Count,
            PageSize = 2,
            PageIndex = 0,
            Items = expectedBooks
        };

        // Setup the mock for the repository's ToPagination method
        _unitOfWorkMock
            .Setup(u => u.BookRepository.ToPagination(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Expression<Func<Book, bool>>>(),
                It.IsAny<Func<IQueryable<Book>, IQueryable<Book>>>(),
                It.IsAny<Expression<Func<Book, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(expectedResult);

        _bookService = new BookService(_unitOfWorkMock.Object, _mapperMock.Object);

        // Act
        var actualResult = await _bookService.Get(0, 2);

        // Assert
        Assert.NotNull(actualResult);
        Assert.Equal(expectedResult.TotalItemsCount, actualResult.TotalItemsCount);
        Assert.Equal(expectedResult.TotalPagesCount, actualResult.TotalPagesCount);
        Assert.Equal(expectedResult.Items.Count, actualResult.Items.Count);


        var expect = expectedResult.Items.ToList();
        var actual = actualResult.Items.ToList();

        for (int i = 0; i < expectedResult.Items.Count; i++)
        {
            Assert.Equal(expect[i].Id, actual[i].Id);
            Assert.Equal(expect[i].Title, actual[i].Title);
            Assert.Equal(expect[i].Description, actual[i].Description);
            Assert.Equal(expect[i].Price, actual[i].Price);
        }
    }

    [Fact]
    public async Task BookService_Add_ShouldAddBook()
    {
        // Arrange
        var bookDTO = new BookDTO
        {
            Title = "New Book",
            Description = "A new book description.",
            Price = 19.99
        };

        var book = new Book
        {
            Id = 1,
            Title = "New Book",
            Description = "A new book description.",
            Price = 19.99
        };

        _mapperMock.Setup(m => m.Map<Book>(bookDTO)).Returns(book);

        _unitOfWorkMock.Setup(u => u.BookRepository.AddAsync(It.IsAny<Book>())).Returns(Task.CompletedTask);

        // Initialize the BookService with the mocked unit of work and mapper
        _bookService = new BookService(_unitOfWorkMock.Object, _mapperMock.Object);

        // Act
        await _bookService.Add(bookDTO, CancellationToken.None);

        // Assert
        //_unitOfWorkMock.Verify(u => u.BookRepository.AddAsync(It.Is<Book>(b => b.Title == bookDTO.Title)), Times.Once);
        _unitOfWorkMock.Verify(u => u.ExecuteTransactionAsync(It.IsAny<Func<Task>>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task BookService_Update_ShouldUpdateBook()
    {
        // Arrange
        var existingBook = new Book
        {
            Id = 1,
            Title = "Existing Book",
            Description = "An existing book description.",
            Price = 25.00
        };

        var updateRequest = new Book
        {
            Id = 1,
            Title = "Updated Book",
            Description = "An updated book description.",
            Price = 30.00
        };

        _unitOfWorkMock.Setup(u => u.BookRepository.FirstOrDefaultAsync(
            It.IsAny<Expression<Func<Book, bool>>>(),
            It.IsAny<Func<IQueryable<Book>, IQueryable<Book>>>()))
                       .ReturnsAsync(existingBook);

        //_unitOfWorkMock.Setup(u => u.BookRepository.Update(It.IsAny<Book>())).Returns(Task.CompletedTask);

        // Initialize the BookService with the mocked unit of work
        _bookService = new BookService(_unitOfWorkMock.Object, _mapperMock.Object);

        // Act
        await _bookService.Update(updateRequest, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(u => u.BookRepository.FirstOrDefaultAsync(
            It.IsAny<Expression<Func<Book, bool>>>(),
            It.IsAny<Func<IQueryable<Book>, IQueryable<Book>>>()), Times.Once);
        //_unitOfWorkMock.Verify(u => u.BookRepository.Update(It.Is<Book>(b => b.Id == updateRequest.Id)), Times.Once);
        //_unitOfWorkMock.Verify(u => u.ExecuteTransactionAsync(It.IsAny<Func<Task>>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task BookService_Delete_ShouldRemoveBook()
    {
        // Arrange
        var bookId = 1;
        var bookToDelete = new Book
        {
            Id = bookId,
            Title = "Book to Delete",
            Description = "A book to delete",
            Price = 20.00
        };

        _unitOfWorkMock.Setup(u => u.BookRepository.FirstOrDefaultAsync(
            It.IsAny<Expression<Func<Book, bool>>>(),
            It.IsAny<Func<IQueryable<Book>, IQueryable<Book>>>()))
                       .ReturnsAsync(bookToDelete);

        //_unitOfWorkMock.Setup(u => u.BookRepository.Delete(It.IsAny<Book>())).Returns(Task.CompletedTask);

        // Initialize the BookService with the mocked unit of work
        _bookService = new BookService(_unitOfWorkMock.Object, _mapperMock.Object);

        // Act
        await _bookService.Delete(bookId, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(u => u.BookRepository.FirstOrDefaultAsync(
            It.IsAny<Expression<Func<Book, bool>>>(),
            It.IsAny<Func<IQueryable<Book>, IQueryable<Book>>>()), Times.Once);
        //_unitOfWorkMock.Verify(u => u.BookRepository.Delete(It.Is<Book>(b => b.Id == bookId)), Times.Once);
        //_unitOfWorkMock.Verify(u => u.ExecuteTransactionAsync(It.IsAny<Func<Task>>(), CancellationToken.None), Times.Once);
    }
}
