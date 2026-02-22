/* 
 * This is the VacanciesController. It manages Job Openings (Vacancies).
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eproject_backend.Data;
using eproject_backend.Models;

namespace eproject_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VacanciesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VacanciesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Vacancies - Fetch all available job openings
        [HttpGet]
        public async Task<IActionResult> GetVacancies()
        {
            // .AsNoTracking() tells EF Core not to track these objects since we're only reading them.
            var vacancies = await _context.Vacancies.AsNoTracking().ToListAsync();
            return Ok(vacancies);
        }

        // GET: api/Vacancies/5 - Fetch a single vacancy by its ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVacancy(int id)
        {
            var vacancy = await _context.Vacancies.FindAsync(id);
            if (vacancy == null) return NotFound();
            return Ok(vacancy);
        }

        // POST: api/Vacancies - Create a new job vacancy (Admin only)
        [HttpPost]
        public async Task<IActionResult> CreateVacancy(Vacancy vacancy)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _context.Vacancies.Add(vacancy);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetVacancy), new { id = vacancy.Id }, vacancy);
        }

        // PUT: api/Vacancies/5 - Update an existing vacancy's details
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVacancy(int id, Vacancy vacancy)
        {
            if (id != vacancy.Id) return BadRequest("ID mismatch");
            _context.Entry(vacancy).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Vacancies.Any(v => v.Id == id)) return NotFound();
                throw;
            }
            return NoContent();
        }

        // DELETE: api/Vacancies/5 - Remove a job vacancy from the system
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVacancy(int id)
        {
            var vacancy = await _context.Vacancies.FindAsync(id);
            if (vacancy == null) return NotFound();
            _context.Vacancies.Remove(vacancy);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

