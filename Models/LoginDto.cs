/* 
 * A 'DTO' (Data Transfer Object) is a simple class used only to move data.
 * The LoginDto is used when the frontend sends the login email and password to the backend.
 */

using System.ComponentModel.DataAnnotations;

namespace eproject_backend.Models
{
    public class LoginDto
    {
        // The email of the user trying to log in
        [Required]
        public string Email { get; set; } = string.Empty;

        // The password of the user trying to log in
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
