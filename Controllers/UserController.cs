/* 
 * This is the UserController. It's used to manage the system's users.
 */

using Microsoft.AspNetCore.Mvc;
using eproject_backend.Data;
using Microsoft.EntityFrameworkCore;

namespace eproject_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/User - Fetch all users in the system (Admin, Employees, etc.)
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            // Simply returns everything in the Users table as a list.
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }
    }
}
