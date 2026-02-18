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
        private readonly IWebHostEnvironment _environment;

        public ApplicationController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // POST: api/Application
        [HttpPost]
        public async Task<IActionResult> Apply([FromForm] Application app)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (app.Cv == null)
                return BadRequest("CV is required.");

            // File validation
            var allowedExtensions = new[] { ".pdf", ".jpg", ".png" };
            var extension = Path.GetExtension(app.Cv.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
                return BadRequest("Only .pdf, .jpg, and .png files are allowed.");

            if (app.Cv.Length > 5 * 1024 * 1024) // 5MB limit
                return BadRequest("Max file size is 5MB.");

            // Save file
            var fileName = Guid.NewGuid() + extension;
            var folderPath = Path.Combine(_environment.WebRootPath ?? "wwwroot", "cv");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            
            var filePath = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await app.Cv.CopyToAsync(stream);
            }

            // Save to DB
            app.CvPath = fileName;
            app.Status = "Pending";

            _context.Applications.Add(app);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Application submitted successfully", applicationId = app.Id });
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

        // PUT: api/Application/approve/{id}
        [HttpPut("approve/{id}")]
        public async Task<IActionResult> Approve(int id)
        {
            var app = await _context.Applications.FindAsync(id);
            if (app == null) return NotFound();

            app.Status = "Approved";

            // Create User account for Employee
            var user = new User
            {
                Name = app.Name,
                Email = app.Email,
                Password = "123456", // Default password
                Role = "Employee"
            };

            // Create Employee record
            var employee = new Employee
            {
                Name = app.Name,
                Email = app.Email,
                ContactNumber = app.Phone,
                EmployeeCode = "EMP-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                Role = "Security Staff", // Default role
                Password = "123456"
            };

            _context.Users.Add(user);
            _context.Employees.Add(employee);
            
            await _context.SaveChangesAsync();

            return Ok(new { message = "Application approved. Employee and User records created." });
        }

        // PUT: api/Application/reject/{id}
        [HttpPut("reject/{id}")]
        public async Task<IActionResult> Reject(int id)
        {
            var app = await _context.Applications.FindAsync(id);
            if (app == null) return NotFound();

            app.Status = "Rejected";
            await _context.SaveChangesAsync();

            return Ok(new { message = "Application rejected." });
        }
    }
}
