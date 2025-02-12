using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Shared.Models;
using CleanArchitecture.Shared.Models.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CleanArchitecture.Web.Controller;

public class BookController(IBookService bookService) : BaseController
{
    private readonly IBookService _bookService = bookService;

    /// <summary>
    /// get a book by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [SwaggerResponse(200, "Book details retrieved successfully.", typeof(BookDTO))]
    [SwaggerResponse(404, "Book not found.")]
    public async Task<IActionResult> Get(int id)
        => Ok(await _bookService.Get(id));

    /// <summary>
    /// get a list of books
    /// </summary>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet]
    [SwaggerResponse(200, "Books retrieved successfully.", typeof(Pagination<BookDTO>))]
    public async Task<IActionResult> Get(int pageIndex = 0, int pageSize = 10)
        => Ok(await _bookService.Get(pageIndex, pageSize));

    /// <summary>
    /// add a book
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPost]
    [SwaggerResponse(201, "Book added successfully.", typeof(BookDTO))]
    [SwaggerResponse(400, "Invalid request.")]
    public async Task<IActionResult> Add(AddBookRequest request, CancellationToken token)
        => Ok(await _bookService.Add(request, token));

    /// <summary>
    /// update a book
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [SwaggerResponse(200, "Book updated successfully.", typeof(BookDTO))]
    [SwaggerResponse(400, "Invalid request.")]
    [SwaggerResponse(404, "Book not found.")]
    public async Task<IActionResult> Update(UpdateBookRequest request, CancellationToken token)
        => Ok(await _bookService.Update(request, token));

    /// <summary>
    /// delete a book by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete("{id}")]
    [SwaggerResponse(200, "Book deleted successfully.")]
    [SwaggerResponse(404, "Book not found.")]
    public async Task<IActionResult> Delete(int id, CancellationToken token)
        => Ok(await _bookService.Delete(id, token));
}
