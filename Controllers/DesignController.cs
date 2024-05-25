using FashionWebsite.Models;
using FashionWebsite.Repository;
using FashionWebsite.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FashionWebsite.Controllers
{
    public class DesignController(IDesignRepository designRepository, UserManager<AppUser> userManager, IHttpContextAccessor contextAccessor) : Controller
    {
        private readonly IDesignRepository _designRepository = designRepository;
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

        public IActionResult Index()
        {

            return View();
        }

        public async Task<IActionResult> MyDesign()
        {
            bool response = await IsUserAuthenticatedAndFashionista();

            if (!response)
                return RedirectToAction("Unauthorized", "Home");

            var result = await _designRepository.GetMyDesigns();

            if (result == null)
                return RedirectToAction("NotFoundPage", "Home");

            return View(result);
        }

        public async Task<IActionResult> ViewDesign(int id)
        {
            var result = await _designRepository.GetDesignById(id);

            if(result == null)
                return RedirectToAction("NotFoundPage", "Home");

            return View(result);
        }

        public async Task<IActionResult> AddDesign()
        {
            bool response = await IsUserAuthenticatedAndFashionista();

            if (!response)
                return RedirectToAction("Unauthorized", "Home");

            var model = new AddDesignViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddDesign(AddDesignViewModel design)
        {
            bool response = await IsUserAuthenticatedAndFashionista();

            if (!response)
                return RedirectToAction("Unauthorized", "Home");

            if (!ModelState.IsValid)
                return View(design);

            var result = await _designRepository.AddDesign(design);

            if(result)
                return RedirectToAction("MyDesign");

            return RedirectToAction("Unexpected", "Home");
        }

        public async Task<IActionResult> EditDesign(int id)
        {
            bool response = await IsUserAuthenticatedAndFashionista();

            if (!response)
                return RedirectToAction("Unauthorized", "Home");

            var result = await _designRepository.GetDesignById(id);

            if(result == null)
                return RedirectToAction("PageNotFound", "Home");

            var model = new EditDesignViewModel
            {
                Id = id,
                Name = result.DesignName,
                Description = result.Description,
                Price = result.Price
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditDesign(EditDesignViewModel design)
        {
            bool response = await IsUserAuthenticatedAndFashionista();

            if (!response)
                return RedirectToAction("Unauthorized", "Home");

            if (!ModelState.IsValid)
                return View(design);

            var result = await _designRepository.EditDesign(design);

            if (result)
                return RedirectToAction("ViewDesign", new {id = design.Id});

            return RedirectToAction("Unauthorized", "Home");
        }

        private async Task<bool> IsUserAuthenticatedAndFashionista()
        {

            var userClaim = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userClaim == null)
                return false;

            string userId = userClaim.Value;

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return false;

            var roles = await _userManager.GetRolesAsync(user);

            if (!roles.Contains("Fashionista"))
                return false;

            return true;
        }
    }
}
