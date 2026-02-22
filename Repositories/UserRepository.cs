/* 
 * The UserRepository handles data operations for User records.
 * Repositories are useful for keeping all database-specific code in one place.
 * This makes the Controllers cleaner and easier to read.
 */

using eproject_backend.Data;
using eproject_backend.Models;

namespace eproject_backend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        // Constructor: The AppDbContext is injected so we can interact with the Users table.
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        // Currently empty, but you could add methods like 'GetUserByEmail' here in the future.
    }
}
