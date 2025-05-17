using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RunGroopWebApp.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Club>> GetAllUserClubs()
        {
            var userId = _httpContextAccessor.HttpContext?.User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return new List<Club>();

            return await _context.Clubs
                .Where(c => c.AppUserId == userId)
                .Include(c => c.Address)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Race>> GetAllUserRaces()
        {
            var userId = _httpContextAccessor.HttpContext?.User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return new List<Race>();

            return await _context.Races
                .Where(r => r.AppUserId == userId)
                .Include(r => r.Address)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<AppUser?> GetUserById(string id) =>
            await _context.Users.FindAsync(id);

        public async Task<AppUser?> GetByIdNoTracking(string id) =>
            await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

        public bool Update(AppUser user)
        {
            _context.Users.Update(user);
            return Save();
        }

        public bool Save() =>
            _context.SaveChanges() > 0;
    }
}
