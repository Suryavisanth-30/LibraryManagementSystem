using LibraryManagement.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminDashboardController : ControllerBase
    {
        private readonly LibraryDbContext context;
        public AdminDashboardController(LibraryDbContext context)
        {
            this.context = context;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetDashboardSummary()
        {
            var total_books = await context.Books.CountAsync();
            var total_users = await context.Users.Where(u => u.Role.RoleName == "User").CountAsync();
            var borrowed_books = await context.BorrowRecord.Where(u =>
            u.Status == "Borrowed").CountAsync();
            var returned_books = await context.BorrowRecord.Where(u =>
            u.Status == "Returned").CountAsync();

            return Ok(new
            {
                total_users,
                total_books,
                borrowed_books,
                returned_books
            }); 
        }
        
            
        }
    }

