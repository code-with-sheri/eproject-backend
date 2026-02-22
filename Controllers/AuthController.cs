/* 
 * This is the AuthController. It handles Authentication tasks, like logging in.
 * In a backend API, Controllers are responsible for receiving requests from the frontend and sending back a response.
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eproject_backend.Data;
using eproject_backend.Models;

namespace eproject_backend.Controllers
{
    // [ApiController] tells ASP.NET that this class handles API requests.
    // [Route("api/[controller]")] means the URL for this controller will be 'api/Auth'
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        // _context is our database connection. 
        // Think of it as a way to talk to our SQL tables.
        private readonly AppDbContext _context;

        // The constructor allows us to 'inject' the database context into this controller.
        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        /* 
         * This function handles a POST request to 'api/Auth/login'.
         * The LoginDto object contains the email and password sent by the frontend.
         */
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            // Step 1: Search the database for a user with the matching email and password.
            // FirstOrDefaultAsync returns the first match it finds, or null if no match exists.
            var user = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.Email == dto.Email &&
                    u.Password == dto.Password);

            // Step 2: If no user was found, send an 'Unauthorized' (401) response back to the frontend.
            if (user == null)
            {
                // This means the email or password was wrong.
                return Unauthorized("Invalid credentials");
            }

            // Step 3: If a user WAS found, send that user's data back with an 'Ok' (200) response.
            // This is how the frontend knows the login was successful.
            return Ok(user);
        }
    }
}