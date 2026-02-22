/* 
 * The ApplicationRepository handles data operations for Job Applications.
 */

using eproject_backend.Data;
using eproject_backend.Models;

namespace eproject_backend.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly AppDbContext _context;

        // Constructor: AppDbContext is injected here to let us talk to the Applications table.
        public ApplicationRepository(AppDbContext context)
        {
            _context = context;
        }

        // Methods like 'AddApplication' or 'GetPending' could be added here to keep Controllers clean.
    }
}
