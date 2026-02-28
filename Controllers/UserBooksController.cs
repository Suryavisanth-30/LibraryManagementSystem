using LibraryManagement.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers
{
    [Authorize(Roles = "User")]
    [Route("api/user/books")]
    [ApiController]
    public class UserBooksController : ControllerBase
    {
        private readonly LibraryDbContext context;

        public UserBooksController(LibraryDbContext context)
        {
            this.context = context;
        }

        // 🔹 View available books
        [HttpGet]
        public async Task<IActionResult> GetAvailableBooks()
        {
            var books = await context.Books
                .Where(b => b.AvailableCopies > 0)
                .Select(b => new
                {
                    b.BookId,
                    b.Title,
                    b.Author,
                    b.Category,
                    b.AvailableCopies
                })
                .ToListAsync();

            return Ok(books);
        }
    }
}
