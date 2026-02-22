/* 
 * This is the EmployeeController. It manages all the logic for Employee records.
 * It allows the Admin to view, create, update, and delete employees.
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eproject_backend.Data;
using eproject_backend.Models;

namespace eproject_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }

        /* 
         * GET request: Fetch the full list of employees from the database.
         */
        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _context.Employees.ToListAsync();
            return Ok(employees);
        }

        /* 
         * GET request: Fetch a single employee by their unique ID.
         */
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            // Step 1: Search the DB for an employee with the matching ID
            var employee = await _context.Employees.FindAsync(id);

            // Step 2: If no employee is found, return a '404 Not Found' response
            if (employee == null) return NotFound();

            // Step 3: Return the found employee
            return Ok(employee);
        }

        /* 
         * GET request: Fetch an employee by their email address.
         * This is useful when the frontend only knows the email of the logged-in user.
         */
        [HttpGet("by-email/{email}")]
        public async Task<IActionResult> GetEmployeeByEmail(string email)
        {
            // We use .FirstOrDefaultAsync() to find the first record that matches the email condition.
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);
            if (employee == null) return NotFound();
            return Ok(employee);
        }

        /* 
         * POST request: Manually create a new employee.
         */
        [HttpPost]
        public async Task<IActionResult> CreateEmployee(Employee employee)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // Returns a '201 Created' response and tells the frontend where the new resource is located.
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
        }

        /* 
         * PUT request: Update an existing employee's details.
         */
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, Employee employee)
        {
            // Basic check: Ensure the ID in the URL matches the ID in the sent data.
            if (id != employee.Id) return BadRequest("ID mismatch");

            // Step 1: If the employee changed their password, we should also update the User record's password.
            // This ensures their login still works with the new password.
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == employee.Email);
            if (user != null)
            {
                user.Password = employee.Password;
                _context.Entry(user).State = EntityState.Modified;
            }

            // Step 2: Tell Entity Framework that the employee object has been modified.
            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                // Step 3: Attempt to save changes to the database.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // If another update happened at the exact same time, we handle the error here.
                if (!_context.Employees.Any(e => e.Id == id)) return NotFound();
                throw;
            }

            return NoContent(); // 204 response (Success, but no data to return)
        }

        /* 
         * DELETE request: Remove an employee and their user account from the system.
         */
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            // Step 1: Find the employee
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            // Step 2: Find and remove the corresponding 'User' account so they can no longer log in.
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == employee.Email);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            // Step 3: Remove the employee record itself
            _context.Employees.Remove(employee);

            // Step 4: Finalise all deletions in the database
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

