using Clean.Architecture.Application.Common.Interfaces;
using Clean.Architecture.Application.Common.Models.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clean.Architecture.Web.Controller
{

    public class BookController(IBookService bookService) : BaseController
    {
        private readonly IBookService _bookService = bookService;

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
            => Ok(await _bookService.Get(id));

        [HttpGet]
        public async Task<IActionResult> Get(int pageIndex = 0, int pageSize = 10)
            => Ok(await _bookService.Get(pageIndex, pageSize));

        [HttpPost]
        public async Task<IActionResult> Add(BookDTO request, CancellationToken token)
        {
            await _bookService.Add(request, token);
            return NoContent();
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update(Book request, CancellationToken token)
        {
            await _bookService.Update(request, token);
            return NoContent();
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id, CancellationToken token)
        {
            await _bookService.Delete(id, token);
            return NoContent();
        }
    }
}
