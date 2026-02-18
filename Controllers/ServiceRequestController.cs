using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eproject_backend.Data;
using eproject_backend.Models;

namespace eproject_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceRequestController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ServiceRequestController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ServiceRequest
        [HttpGet]
        public async Task<IActionResult> GetServiceRequests()
        {
            var requests = await _context.ServiceRequests.ToListAsync();
            return Ok(requests);
        }

        // POST: api/ServiceRequest
        [HttpPost]
        public async Task<IActionResult> CreateServiceRequest(ServiceRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _context.ServiceRequests.Add(request);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Service request submitted successfully" });
        }

        // DELETE: api/ServiceRequest/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceRequest(int id)
        {
            var request = await _context.ServiceRequests.FindAsync(id);
            if (request == null) return NotFound();
            _context.ServiceRequests.Remove(request);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
