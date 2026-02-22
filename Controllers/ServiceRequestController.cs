/* 
 * This is the ServiceRequestController. It handles all requests related to services.
 * In a backend API, Controllers receive HTTP requests (GET, POST, PUT, DELETE) and perform actions.
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eproject_backend.Data;
using eproject_backend.Models;

namespace eproject_backend.Controllers
{
    // [ApiController] helps automatically handle standard API tasks like validating model data.
    // [Route("api/[controller]")] means the endpoint for this controller will be 'api/ServiceRequest'.
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceRequestController : ControllerBase
    {
        private readonly AppDbContext _context;

        // The constructor allows our app to 'inject' (pass in) the database context to let us talk to the DB.
        public ServiceRequestController(AppDbContext context)
        {
            _context = context;
        }

        /* 
         * GET request: Fetch all service requests from the database.
         * The URL is 'api/ServiceRequest' (GET)
         */
        [HttpGet]
        public async Task<IActionResult> GetServiceRequests()
        {
            // .ToListAsync() tells Entity Framework to convert the 'ServiceRequests' table into a list of objects.
            var requests = await _context.ServiceRequests.ToListAsync();
            return Ok(requests); // 200 Ok response
        }

        /* 
         * GET request: Fetch service requests that are assigned to a specific employee.
         * The URL is 'api/ServiceRequest/assigned/{employeeId}'
         */
        [HttpGet("assigned/{employeeId}")]
        public async Task<IActionResult> GetAssignedTasks(int employeeId)
        {
            // .Where() allows us to filter the list to only find matches for the 'employeeId'.
            var requests = await _context.ServiceRequests
                .Where(r => r.AssignedEmployeeId == employeeId)
                .ToListAsync();
            return Ok(requests);
        }

        /* 
         * POST request: Create a new service request (e.g., when a user submits a form).
         * The URL is 'api/ServiceRequest' (POST)
         */
        [HttpPost]
        public async Task<IActionResult> CreateServiceRequest(ServiceRequest request)
        {
            // Check if the request data sent from the frontend is valid (e.g., required fields are filled).
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Add the new request object to our database table
            _context.ServiceRequests.Add(request);

            // Save the changes to the SQL Server database
            await _context.SaveChangesAsync();
            return Ok(new { message = "Service request submitted successfully" });
        }

        /* 
         * DELETE request: Remove a specific service request using its ID.
         * The URL is 'api/ServiceRequest/{id}'
         */
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceRequest(int id)
        {
            // Step 1: Find the request by its primary key (ID)
            var request = await _context.ServiceRequests.FindAsync(id);

            // Step 2: If the request doesn't exist, return a '404 Not Found' response.
            if (request == null) return NotFound();

            // Step 3: Remove the found record from the database table
            _context.ServiceRequests.Remove(request);

            // Step 4: Save the changes to finalise the deletion in SQL
            await _context.SaveChangesAsync();
            return NoContent(); // 204 No Content response (indicates success without returning data)
        }

        /* 
         * PUT request: Update a service request to assign it to an employee.
         * The URL is 'api/ServiceRequest/assign/{id}'
         */
        [HttpPut("assign/{id}")]
        public async Task<IActionResult> Assign(int id, [FromBody] int employeeId)
        {
            // Step 1: Find the service request we want to assign
            var request = await _context.ServiceRequests.FindAsync(id);
            if (request == null) return NotFound();

            // Step 2: Check if the employee being assigned actually exists in the 'Employees' table
            var employeeExists = await _context.Employees.AnyAsync(e => e.Id == employeeId);
            if (!employeeExists) return BadRequest("Employee not found");

            // Step 3: Update the service request's properties
            request.AssignedEmployeeId = employeeId;
            request.Status = "Assigned";

            // Step 4: Save the updated record to the database
            await _context.SaveChangesAsync();

            return Ok(new { message = "Service request assigned successfully" });
        }
    }
}

