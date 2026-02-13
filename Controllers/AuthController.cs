using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eproject_backend.Data;
using eproject_backend.Models;

namespace eproject_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.Email == dto.Email &&
                    u.Password == dto.Password);

            if (user == null)
                return Unauthorized("Invalid credentials");

            return Ok(user);
        }
    }
}