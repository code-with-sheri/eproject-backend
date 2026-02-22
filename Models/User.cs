/* 
 * The User model represents a person who can log into the system.
 * This includes both Administrators and Employees.
 */

using System.Text.Json.Serialization;

namespace eproject_backend.Models
{
    public class User
    {
        // Unique ID for each user (Primary Key in the database)
        public int Id { get; set; }

        // Full name of the user
        public string? Name { get; set; }

        // Email address used for logging in
        public string? Email { get; set; }
        
        // [JsonIgnore] is a security feature. 
        // it tells the server NEVER to send the password back to the frontend in API responses.
        [JsonIgnore]
        public string? Password { get; set; }

        // The role can be "Admin" or "Employee". 
        // This determines what pages they can see in the frontend.
        public string? Role { get; set; }
    }
}
