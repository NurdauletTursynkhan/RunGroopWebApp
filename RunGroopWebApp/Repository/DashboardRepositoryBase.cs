using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Repository
{
    public class DashboardRepositoryBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public async Task<List<Club>> GetAllUserClubs()
        {
            var userId = _httpContextAccessor.HttpContext?.User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return new List<Club>();

            return await _context.Clubs
                .Where(c => c.AppUserId == userId)
                .Include(c => c.Address)
                .AsNoTracking()
                .ToListAsync();
        }

        // Дополнительный метод для Dashboard без явного userId
        public async Task<List<Club>> GetAllUserClubs()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.GetUserId();
            if (string.IsNullOrEmpty(userId)) return new List<Club>();

            return await _context.Clubs
                .Where(c => c.AppUserId == userId)
                .Include(c => c.Address)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Race>> GetAllUserRaces()
        {
            var userId = _httpContextAccessor.HttpContext?.User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return new List<Race>();

            return await _context.Races
                .Where(r => r.AppUserId == userId)
                .Include(r => r.Address)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Race>> GetAllUserRaces()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.GetUserId();
            if (string.IsNullOrEmpty(userId)) return new List<Race>();

            return await _context.Races
                .Where(r => r.AppUserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<AppUser?> GetByIdNoTracking(string id) =>
            await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

        public async Task<AppUser?> GetByIdNoTracking(string id)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<AppUser?> GetUserById(string id) =>
            await _context.Users.FindAsync(id);

        public async Task<AppUser?> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<List<Club>> GetUserClubs(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return new List<Club>();

            return await _context.Clubs
                .Where(c => c.AppUserId == userId)
                .Include(c => c.Address)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Race>> GetUserRaces(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return new List<Race>();

            return await _context.Races
                .Where(r => r.AppUserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }

        public bool Save() =>
            _context.SaveChanges() > 0;

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public bool Update(AppUser user)
        {
            _context.Users.Update(user);
            return Save();
        }

        public bool Update(AppUser user)
        {
            _context.Users.Update(user);
            return Save();
        }
    }
}