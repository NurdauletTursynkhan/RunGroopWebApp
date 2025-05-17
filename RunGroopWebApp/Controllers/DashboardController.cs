using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IDashboardRepository _dashboardRepository;

        // ✅ Конструктор
        public DashboardController(IDashboardRepository dashboardRepository, UserManager<AppUser> userManager)
        {
            _dashboardRepository = dashboardRepository;
            _userManager = userManager;
        }

        // ✅ Главная страница пользователя (Dashboard)
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Index", "Home");

            var races = await _dashboardRepository.GetAllUserRaces();
            var clubs = await _dashboardRepository.GetAllUserClubs();

            var dashboardViewModel = new DashboardViewModel
            {
                Races = races,
                Clubs = clubs
            };

            return View(dashboardViewModel);
        }
    }
}
