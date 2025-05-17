using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Data.Enum;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository _clubRepository;
        private readonly IPhotoService _photoService;

        public ClubController(IClubRepository clubRepository, IPhotoService photoService)
        {
            _clubRepository = clubRepository;
            _photoService = photoService;
        }

        [HttpGet]
        [Route("RunningClubs")]
        public async Task<IActionResult> Index(int category = -1, int page = 1, int pageSize = 6)
        {
            if (page < 1 || pageSize < 1) return NotFound();

            var clubs = category switch
            {
                -1 => await _clubRepository.GetSliceAsync((page - 1) * pageSize, pageSize),
                _ => await _clubRepository.GetClubsByCategoryAndSliceAsync((ClubCategory)category, (page - 1) * pageSize, pageSize),
            };

            var count = category switch
            {
                -1 => await _clubRepository.GetCountAsync(),
                _ => await _clubRepository.GetCountByCategoryAsync((ClubCategory)category),
            };

            return View(new IndexClubViewModel
            {
                Clubs = clubs,
                Page = page,
                PageSize = pageSize,
                TotalClubs = count,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize),
                Category = category,
            });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var curUserId = HttpContext.User.GetUserId();
            if (string.IsNullOrEmpty(curUserId)) return Unauthorized();

            return View(new CreateClubViewModel { AppUserId = curUserId });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel clubVM)
        {
            if (!ModelState.IsValid) return View(clubVM);

            if (clubVM.Image == null)
            {
                ModelState.AddModelError("Image", "Image file is required.");
                return View(clubVM);
            }

            var result = await _photoService.AddPhotoAsync(clubVM.Image);
            if (result.Error != null)
            {
                ModelState.AddModelError("Image", "Photo upload failed");
                return View(clubVM);
            }

            var club = new Club
            {
                Title = clubVM.Title,
                Description = clubVM.Description,
                Image = result.Url?.ToString() ?? "",
                ClubCategory = clubVM.ClubCategory,
                AppUserId = clubVM.AppUserId,
                Address = new Address
                {
                    Street = clubVM.Address?.Street ?? "",
                    City = clubVM.Address?.City ?? "",
                    State = clubVM.Address?.State ?? "",
                }
            };

            _clubRepository.Add(club);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("club/{runningClub}/{id}")]
        public async Task<IActionResult> DetailClub(int id, string runningClub)
        {
            var club = await _clubRepository.GetByIdAsync(id);
            return club == null ? NotFound() : View(club);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel clubVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", clubVM);
            }

            var userClub = await _clubRepository.GetByIdAsyncNoTracking(id);
            if (userClub == null) return View("Error");

            if (clubVM.Image == null)
            {
                ModelState.AddModelError("Image", "Image file is required.");
                return View(clubVM);
            }

            var photoResult = await _photoService.AddPhotoAsync(clubVM.Image);
            if (photoResult.Error != null)
            {
                ModelState.AddModelError("Image", "Photo upload failed");
                return View(clubVM);
            }

            if (!string.IsNullOrEmpty(userClub.Image))
            {
                await _photoService.DeletePhotoAsync(userClub.Image);
            }

            var club = new Club
            {
                Id = id,
                Title = clubVM.Title,
                Description = clubVM.Description,
                Image = photoResult.Url.ToString(),
                AddressId = clubVM.AddressId,
                Address = clubVM.Address ?? new Address(),
            };

            _clubRepository.Update(club);
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var clubDetails = await _clubRepository.GetByIdAsync(id);
            if (clubDetails == null) return View("Error");

            if (!string.IsNullOrEmpty(clubDetails.Image))
            {
                await _photoService.DeletePhotoAsync(clubDetails.Image);
            }

            _clubRepository.Delete(clubDetails);
            return RedirectToAction("Index");
        }
    }
}
