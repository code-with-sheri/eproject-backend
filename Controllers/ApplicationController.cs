using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eproject_backend.Data;
using eproject_backend.Models;

namespace eproject_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ApplicationController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/Application
        [HttpPost]
        public async Task<IActionResult> Apply([FromForm] Application app)
        {
            // show exact validation errors
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // check cv
            if (app.cv == null)
                return BadRequest("CV is null");

            // save file
            var fileName = Guid.NewGuid() + Path.GetExtension(app.cv.FileName);
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "cv");
            var filePath = Path.Combine(folderPath, fileName);

            Directory.CreateDirectory(folderPath);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await app.cv.CopyToAsync(stream);
            }

            // save db
            app.CvPath = fileName;
            app.Status = "Pending";

            _context.Applications.Add(app);
            await _context.SaveChangesAsync();

            return Ok(app);
        }

        // GET: api/Application/pending
        [HttpGet("pending")]
        public async Task<IActionResult> GetPending()
        {
            var pending = await _context.Applications
                .Where(a => a.Status == "Pending")
                .ToListAsync();

            return Ok(pending);
        }

        // PUT: api/Application/approve/5
        [HttpPut("approve/{id}")]
        public async Task<IActionResult> Approve(int id)
        {
            var app = await _context.Applications.FindAsync(id);
            if (app == null) return NotFound();

            app.Status = "Approved";

            var user = new User
            {
                Name = app.name,     // lowercase
                Email = app.email,   // lowercase
                Password = "1234",
                Role = "Employee"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Approved and employee created");
        }

        // PUT: api/Application/reject/5
        [HttpPut("reject/{id}")]
        public async Task<IActionResult> Reject(int id)
        {
            var app = await _context.Applications.FindAsync(id);
            if (app == null) return NotFound();

            app.Status = "Rejected";
            await _context.SaveChangesAsync();

            return Ok("Rejected");
        }
    }
}