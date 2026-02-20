using eproject_backend.Data;
using eproject_backend.Models;

namespace eproject_backend.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly AppDbContext _context;

        public ApplicationRepository(AppDbContext context)
        {
            _context = context;
        }
    }
}
