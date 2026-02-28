using LibraryManagement.Data;
using LibraryManagement.DTO;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/admin/books")]
    [ApiController]
    public class AdminBooksController : ControllerBase
    {
        private readonly LibraryDbContext context;

        public AdminBooksController(LibraryDbContext context)
        {
            this.context = context;
        }

        // 🔹 Get all books
        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await context.Books.ToListAsync();
            return Ok(books);
        }

        // 🔹 Add book
        [HttpPost]
        public async Task<IActionResult> AddBook(AddBookDto dto)
        {
            var book = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                Category = dto.Category,
                TotalCopies = dto.TotalCopies,
                AvailableCopies = dto.TotalCopies
            };

            context.Books.Add(book);
            await context.SaveChangesAsync();

            return Ok(new { message = "Book added successfully" });
        }

        // 🔹 Update book
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, UpdateBookDto dto)
        {
            var book = await context.Books.FindAsync(id);
            if (book == null)
                return NotFound();

            book.Title = dto.Title;
            book.Author = dto.Author;
            book.Category = dto.Category;
            book.TotalCopies = dto.TotalCopies;
            book.AvailableCopies = dto.AvailableCopies;

            await context.SaveChangesAsync();
            return Ok(new { message = "Book updated successfully" });
        }

        // 🔹 Delete book
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await context.Books.FindAsync(id);
            if (book == null)
                return NotFound();

            context.Books.Remove(book);
            await context.SaveChangesAsync();

            return Ok(new { message = "Book deleted successfully" });
        }
    }
}
