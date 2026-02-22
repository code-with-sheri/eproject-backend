/* 
 * This is the ApplicationController. It handles Job Applications submitted by users.
 * It includes logic for uploading a CV file and approving an application to create an employee record.
 */

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

        // Constructor: We inject the DB context and the WebHostEnvironment (needed for saving files).
        public ApplicationController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        /* 
         * POST request: Submit a new job application.
         * The [FromForm] attribute is used because we are sending a file (the CV).
         */
        [HttpPost]
        public async Task<IActionResult> Apply([FromForm] Application app)
        {
            // Step 1: Basic validation
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (app.Cv == null) return BadRequest("CV is required.");

            // Step 2: Check file extension (only allow PDF, JPG, or PNG)
            var allowedExtensions = new[] { ".pdf", ".jpg", ".png" };
            var extension = Path.GetExtension(app.Cv.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
                return BadRequest("Only .pdf, .jpg, and .png files are allowed.");

            // Step 3: Check file size (limit to 5MB)
            if (app.Cv.Length > 5 * 1024 * 1024) 
                return BadRequest("Max file size is 5MB.");

            // Step 4: Save the file to the 'wwwroot/cv' folder
            // We give it a unique name using Guid.NewGuid() so files don't overwrite each other.
            var fileName = Guid.NewGuid() + extension;
            var folderPath = Path.Combine(_environment.WebRootPath ?? "wwwroot", "cv");
            
            // Ensure the 'cv' folder exists
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            
            var filePath = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await app.Cv.CopyToAsync(stream); // Actually save the file to disk
            }

            // Step 5: Save the application details to the database
            app.CvPath = fileName; // Store only the filename in the DB
            app.Status = "Pending"; // All new applications start as 'Pending'

            _context.Applications.Add(app);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Application submitted successfully", applicationId = app.Id });
        }

        /* 
         * GET request: Fetch all job applications that are currently 'Pending'.
         */
        [HttpGet("pending")]
        public async Task<IActionResult> GetPending()
        {
            var pending = await _context.Applications
                .Where(a => a.Status == "Pending")
                .ToListAsync();

            return Ok(pending);
        }

        /* 
         * PUT request: Approve a job application.
         * When approved, we automatically create a User account and an Employee record.
         */
        [HttpPut("approve/{id}")]
        public async Task<IActionResult> Approve(int id)
        {
            // Step 1: Find the application
            var app = await _context.Applications.FindAsync(id);
            if (app == null) return NotFound();

            // Step 2: Update status to 'Approved'
            app.Status = "Approved";

            // Step 3: Create a User account so the person can log in.
            // Check if a user with this email already exists first.
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == app.Email);
            if (existingUser == null)
            {
                var user = new User
                {
                    Name = app.Name,
                    Email = app.Email,
                    Password = "123456", // Give them a default password
                    Role = "Employee"
                };
                _context.Users.Add(user);
            }

            // Step 4: Create the Employee record
            var employee = new Employee
            {
                Name = app.Name,
                Email = app.Email,
                ContactNumber = app.Phone,
                EmployeeCode = "EMP-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                Role = "Security Staff", // Default job role
                Password = "123456"
            };
            _context.Employees.Add(employee);

            // Step 5: Remove the job vacancy from public display (since it's now filled)
            var vacancy = await _context.Vacancies.FindAsync(app.VacancyId);
            if (vacancy != null)
            {
                _context.Vacancies.Remove(vacancy);
            }
            
            // Step 6: Save all changes (updates status, adds user, adds employee, removes vacancy)
            await _context.SaveChangesAsync();

            return Ok(new { message = "Application approved. Employee and User records created, and vacancy removed." });
        }

        /* 
         * PUT request: Reject a job application.
         */
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
