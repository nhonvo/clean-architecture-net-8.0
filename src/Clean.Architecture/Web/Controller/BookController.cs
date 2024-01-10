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
        public async Task<IActionResult> Add(BookDTO request)
            => Ok(await _bookService.Add(request));

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update(Book request)
            => Ok(await _bookService.Update(request));

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
            => Ok(await _bookService.Delete(id));
    }
}
