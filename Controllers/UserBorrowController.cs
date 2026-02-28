using System.Security.Claims;
using LibraryManagement.Data;
using LibraryManagement.DTO;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers
{
    [Authorize(Roles = "User")]
    [Route("api/user/borrow")]
    [ApiController]
    public class UserBorrowController : ControllerBase
    {
        private readonly LibraryDbContext context;

        public UserBorrowController(LibraryDbContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public async Task<IActionResult> BorrowBook(BorrowBookDto dto)
        {
            // 🔹 Get logged-in user id from JWT
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            // 🔹 Check book
            var book = await context.Books.FindAsync(dto.BookId);
            if (book == null)
                return NotFound(new { message = "Book not found" });

            if (book.AvailableCopies <= 0)
                return BadRequest(new { message = "Book not available" });

            // 🔹 Prevent duplicate borrow
            var alreadyBorrowed = await context.BorrowRecord.AnyAsync(b =>
                b.BookId == dto.BookId &&
                b.UserId == userId &&
                b.Status == "Borrowed"
            );

            if (alreadyBorrowed)
                return BadRequest(new { message = "You already borrowed this book" });

            // 🔹 Borrow book
            var record = new BorrowRecord
            {
                UserId = userId,
                BookId = dto.BookId,
                BorrowDate = DateTime.Now,
                Status = "Borrowed"
            };

            context.BorrowRecord.Add(record);
            book.AvailableCopies -= 1;

            await context.SaveChangesAsync();

            return Ok(new { message = "Book borrowed successfully" });
        }
    
    [HttpPost("return")]
        public async Task<IActionResult> ReturnBook(ReturnBookDto dto)
        {
            // 🔹 Get logged-in user id from JWT
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            // 🔹 Find active borrow record
            var record = await context.BorrowRecord.FirstOrDefaultAsync(b =>
                b.UserId == userId &&
                b.BookId == dto.BookId &&
                b.Status == "Borrowed"
            );

            if (record == null)
                return BadRequest(new { message = "No active borrow record found" });

            // 🔹 Update borrow record
            record.Status = "Returned";
            record.ReturnDate = DateTime.Now;

            // 🔹 Increase available copies
            var book = await context.Books.FindAsync(dto.BookId);
            book.AvailableCopies += 1;

            await context.SaveChangesAsync();

            return Ok(new { message = "Book returned successfully" });
        }
        [HttpGet("history")]
        public async Task<IActionResult> GetMyBorrowHistory()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var history = await context.BorrowRecord
                .Where(b => b.UserId == userId)
                .Select(b => new
                {
                    b.BorrowId,
                    b.BookId,
                    b.BorrowDate,
                    b.ReturnDate,
                    b.Status
                })
                .ToListAsync();

            return Ok(history);
        }
    }
    }