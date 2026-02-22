/* 
 * This is the ServicesController. It manages the company's offerings (e.g., Home Security, Office Security).
 * It provides standard CRUD (Create, Read, Update, Delete) operations.
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eproject_backend.Data;
using eproject_backend.Models;

namespace eproject_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ServicesController(AppDbContext context)
        {
            _context = context;
        }

        /* 
         * GET request: Fetch all services offered by the company.
         * The URL is 'api/Services' (GET).
         */
        [HttpGet]
        public async Task<IActionResult> GetServices()
        {
            // .AsNoTracking() is a performance tip for read-only operations. 
            // It tells Entity Framework not to track changes to these objects.
            var services = await _context.Services.AsNoTracking().ToListAsync();
            return Ok(services);
        }

        /* 
         * GET request: Fetch a single service by its unique ID.
         */
        [HttpGet("{id}")]
        public async Task<IActionResult> GetService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return NotFound();
            return Ok(service);
        }

        /* 
         * POST request: Add a new service to the company offerings.
         */
        [HttpPost]
        public async Task<IActionResult> CreateService(Service service)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            // Returns '201 Created' and points to the location of the new service.
            return CreatedAtAction(nameof(GetService), new { id = service.Id }, service);
        }

        /* 
         * PUT request: Update an existing service.
         */
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateService(int id, Service service)
        {
            if (id != service.Id) return BadRequest("ID mismatch");

            // Mark the object as modified in the database tracking system.
            _context.Entry(service).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Services.Any(s => s.Id == id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        /* 
         * DELETE request: Remove a service from the offerings.
         */
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            // Step 1: Find the service
            var service = await _context.Services.FindAsync(id);
            if (service == null) return NotFound();

            // Step 2: Remove the service from the DB table
            _context.Services.Remove(service);

            // Step 3: Save changes permanently
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

