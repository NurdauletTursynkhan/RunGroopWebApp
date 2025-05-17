using RunGroopWebApp.Models;

namespace RunGroopWebApp.Interfaces
{
    public interface IDashboardRepository
    {
        Task<List<Club>> GetAllUserClubs();
        Task<List<Race>> GetAllUserRaces();
        Task<AppUser?> GetUserById(string id);
        Task<AppUser?> GetByIdNoTracking(string id);
        bool Update(AppUser user);
        bool Save();
    }
}