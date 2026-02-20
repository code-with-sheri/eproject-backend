using eproject_backend.Data;
using eproject_backend.Models;

namespace eproject_backend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
    }
}
