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

        // GET: api/Services
        [HttpGet]
        public async Task<IActionResult> GetServices()
        {
            var services = await _context.Services.ToListAsync();
            return Ok(services);
        }

        // GET: api/Services/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return NotFound();
            return Ok(service);
        }

        // POST: api/Services
        [HttpPost]
        public async Task<IActionResult> CreateService(Service service)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _context.Services.Add(service);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetService), new { id = service.Id }, service);
        }

        // PUT: api/Services/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateService(int id, Service service)
        {
            if (id != service.Id) return BadRequest();
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

        // DELETE: api/Services/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return NotFound();
            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
