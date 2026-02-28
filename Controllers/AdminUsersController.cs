using LibraryManagement.Data;
using LibraryManagement.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/admin/users")]
    [ApiController]
    public class AdminUsersController : ControllerBase
    {
        private readonly LibraryDbContext context;

        public AdminUsersController(LibraryDbContext context)
        {
            this.context = context;
        }

        // 🔹 Get all users (except Admin)
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await context.Users
                .Include(u => u.Role)
                .Where(u => u.Role.RoleName == "User")
                .Select(u => new
                {
                    u.UserId,
                    u.FullName,
                    u.Email,
                    u.IsActive
                })
                .ToListAsync();

            return Ok(users);
        }

        // 🔹 Activate / Deactivate user
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateUserStatus(int id, UpdateUserStatusDto dto)
        {
            var user = await context.Users.FindAsync(id);
            if (user == null)
                return NotFound(new { message = "User not found" });

            user.IsActive = dto.IsActive;
            await context.SaveChangesAsync();

            return Ok(new
            {
                message = dto.IsActive ? "User activated" : "User deactivated"
            });
        }
    }
}
