using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using RunGroopWebApp.Data;
using RunGroopWebApp.Helpers;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using UserRoles = RunGroopWebApp.Models.UserRoles;

namespace RunGroopWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger; private readonly IClubRepository _clubRepository; private readonly UserManager<AppUser> _userManager; private readonly SignInManager<AppUser> _signInManager; private readonly ILocationService _locationService; private readonly IConfiguration _config; private readonly HttpClient _httpClient; private readonly IStringLocalizer<SharedResource> _localizer;
        public HomeController(
    ILogger<HomeController> logger,
    IClubRepository clubRepository,
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    ILocationService locationService,
    IConfiguration config,
    HttpClient httpClient,
    IStringLocalizer<SharedResource> localizer)
        {
            _logger = logger;
            _clubRepository = clubRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _locationService = locationService;
            _config = config;
            _httpClient = httpClient;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index()
        {
            var ipInfo = new IPInfo();
            var homeViewModel = new HomeViewModel();

            try
            {
                string url = "https://ipinfo.io?token=" + _config.GetValue<string>("IPInfoToken");
                var info = await _httpClient.GetStringAsync(url);
                ipInfo = JsonConvert.DeserializeObject<IPInfo>(info) ?? new IPInfo();

                if (!string.IsNullOrEmpty(ipInfo.Country))
                {
                    RegionInfo region = new RegionInfo(ipInfo.Country);
                    ipInfo.Country = region.EnglishName;
                }

                homeViewModel.City = ipInfo.City;
                homeViewModel.State = ipInfo.Region;

                if (!string.IsNullOrEmpty(homeViewModel.City))
                {
                    homeViewModel.Clubs = await _clubRepository.GetClubByCity(homeViewModel.City);
                }

                ViewBag.Title = _localizer["Welcome"];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении IP-данных.");
                homeViewModel.Clubs = null;
            }

            return View(homeViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(HomeViewModel homeVM)
        {
            var createVM = homeVM.Register;
            if (!ModelState.IsValid) return View(homeVM);

            var user = await _userManager.FindByEmailAsync(createVM.Email);
            if (user != null)
            {
                ModelState.AddModelError("Register.Email", _localizer["EmailAlreadyUsed"]);
                return View(homeVM);
            }

            var userLocation = await _locationService.GetCityByZipCode(createVM.ZipCode ?? 0);
            if (userLocation == null)
            {
                ModelState.AddModelError("Register.ZipCode", _localizer["ZipNotFound"]);
                return View(homeVM);
            }

            var newUser = new AppUser
            {
                UserName = createVM.UserName,
                Email = createVM.Email,
                Address = new Address()
                {
                    State = userLocation.StateCode,
                    City = userLocation.CityName,
                    ZipCode = createVM.ZipCode ?? 0,
                }
            };

            var newUserResponse = await _userManager.CreateAsync(newUser, createVM.Password);
            if (!newUserResponse.Succeeded)
            {
                foreach (var error in newUserResponse.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(homeVM);
            }

            await _signInManager.SignInAsync(newUser, isPersistent: false);
            await _userManager.AddToRoleAsync(newUser, UserRoles.User);

            return RedirectToAction("Index", "Club");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
