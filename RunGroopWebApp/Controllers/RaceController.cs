using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Data.Enum;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceRepository _raceRepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RaceController(IRaceRepository raceRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
            _raceRepository = raceRepository;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var race = await _raceRepository.GetByIdAsync(id);
            if (race == null) return View("Error");

            var raceVM = new EditRaceViewModel
            {
                Id = race.Id,
                Title = race.Title,
                Description = race.Description,
                RaceCategory = race.RaceCategory,
                AddressId = race.AddressId,
                Address = race.Address
            };

            return View(raceVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditRaceViewModel raceVM)
        {
            if (!ModelState.IsValid) return View(raceVM);

            var race = await _raceRepository.GetByIdAsyncNoTracking(id);
            if (race == null) return View("Error");

            var updatedRace = new Race
            {
                Id = id,
                Title = raceVM.Title,
                Description = raceVM.Description,
                RaceCategory = raceVM.RaceCategory,
                AddressId = raceVM.AddressId,
                Address = raceVM.Address
            };

            _raceRepository.Update(updatedRace);
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var race = await _raceRepository.GetByIdAsync(id);
            if (race == null) return View("Error");

            _raceRepository.Delete(race);
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> Index(int category = -1, int page = 1, int pageSize = 6)
        {
            if (page < 1 || pageSize < 1)
            {
                return NotFound();
            }

            var races = category switch
            {
                -1 => await _raceRepository.GetSliceAsync((page - 1) * pageSize, pageSize),
                _ => await _raceRepository.GetRacesByCategoryAndSliceAsync((RaceCategory)category, (page - 1) * pageSize, pageSize),
            };

            var count = category switch
            {
                -1 => await _raceRepository.GetCountAsync(),
                _ => await _raceRepository.GetCountByCategoryAsync((RaceCategory)category),
            };

            var viewModel = new IndexRaceViewModel
            {
                Races = races,
                Page = page,
                PageSize = pageSize,
                TotalRaces = count,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize),
                Category = category,
            };

            return View(viewModel);
        }

        [HttpGet]
        [Route("event/{runningRace}/{id}")]
        public async Task<IActionResult> DetailRace(int id, string runningRace)
        {
            var race = await _raceRepository.GetByIdAsync(id);
            return race == null ? NotFound() : View(race);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var curUserID = _httpContextAccessor.HttpContext?.User.GetUserId();
            var createRaceViewModel = new CreateRaceViewModel { AppUserId = curUserID ?? string.Empty };
            return View(createRaceViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRaceViewModel raceVM)
        {
            if (ModelState.IsValid)
            {
                var result = raceVM.Image != null ? await _photoService.AddPhotoAsync(raceVM.Image) : null;

                var race = new Race
                {
                    Title = raceVM.Title,
                    Description = raceVM.Description,
                    Image = result?.Url?.ToString() ?? string.Empty,
                    AppUserId = raceVM.AppUserId,
                    RaceCategory = raceVM.RaceCategory,
                    Address = new Address
                    {
                        Street = raceVM.Address.Street,
                        City = raceVM.Address.City,
                        State = raceVM.Address.State,
                    }
                };
                _raceRepository.Add(race);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View(raceVM);
        }
    }
}
