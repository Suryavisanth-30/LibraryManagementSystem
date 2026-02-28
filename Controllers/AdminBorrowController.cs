using LibraryManagement.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Models;

namespace LibraryManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/admin/borrow")]
    [ApiController]
    public class AdminBorrowController : ControllerBase
    {
        private readonly LibraryDbContext context;

        public AdminBorrowController(LibraryDbContext context)
        {
            this.context = context;
        }

        // 📄 Get all borrow records
        [HttpGet]
        public async Task<IActionResult> GetBorrowRecords()
        {
            var records = await context.BorrowRecord
                .Include(b => b.User)
                .Include(b => b.Book)
                .ToListAsync();

            return Ok(records);
        }
    }
}
