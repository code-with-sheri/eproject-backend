/* 
 * This is the AdminController. It handles operations specifically for users with the 'Admin' role.
 * In this project, it's used to provide statistics for the Admin Dashboard.
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eproject_backend.Data;

namespace eproject_backend.Controllers
{
    // The [ApiController] attribute tells ASP.NET that this class handles requests and responses as an API.
    // The [Route("api/[controller]")] attribute sets the URL for this controller to 'api/Admin'.
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        // _context is our database connection that we use to fetch data.
        private readonly AppDbContext _context;

        // Constructor: The AppDbContext is injected here to let us interact with the database tables.
        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        /* 
         * This function handles a GET request to 'api/Admin/stats'.
         * It calculates and returns simple counts for various categories (Employees, Clients, etc.).
         */
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            // We use .CountAsync() to ask the database to count how many records are in each table.
            // This is efficient because the counting happens in the database, not in our code.
            var stats = new
            {
                // Count all employees in the Employees table
                TotalEmployees = await _context.Employees.CountAsync(),
                
                // Count all clients in the Clients table
                TotalClients = await _context.Clients.CountAsync(),
                
                // Count all job openings (vacancies) in the Vacancies table
                TotalVacancies = await _context.Vacancies.CountAsync(),
                
                // Count only job applications that have their status set to "Pending"
                PendingApplications = await _context.Applications.CountAsync(a => a.Status == "Pending"),
                
                // Count all service requests in the ServiceRequests table
                TotalServiceRequests = await _context.ServiceRequests.CountAsync()
            };

            // Send the results back as an 'Ok' (200) response with the 'stats' object.
            return Ok(stats);
        }
    }
}

