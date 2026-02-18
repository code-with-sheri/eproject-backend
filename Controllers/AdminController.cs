using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eproject_backend.Data;

namespace eproject_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var stats = new
            {
                TotalEmployees = await _context.Employees.CountAsync(),
                TotalClients = await _context.Clients.CountAsync(),
                TotalVacancies = await _context.Vacancies.CountAsync(),
                PendingApplications = await _context.Applications.CountAsync(a => a.Status == "Pending"),
                TotalServiceRequests = await _context.ServiceRequests.CountAsync()
            };

            return Ok(stats);
        }
    }
}
