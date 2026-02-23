using LibraryManagement.Data;
using LibraryManagement.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly LibraryDbContext context;
        public AuthController(LibraryDbContext context)
        {
            this.context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var user = await context.Users
                .Include(u => u.Role)  
                .FirstOrDefaultAsync(u =>
                    u.Email == request.Email &&
                    u.Password == request.Password &&
                    u.IsActive
                );

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            var response = new LoginResponseDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Role = user.Role.RoleName,
                Message = "Login successful"
            };

            return Ok(response);
        }
    }
}
