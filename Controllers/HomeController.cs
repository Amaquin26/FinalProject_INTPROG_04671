using FashionWebsite.Models;
using FashionWebsite.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FashionWebsite.Controllers
{
    public class HomeController(IHomeRepository homeRepository) : Controller
    {
        private readonly IHomeRepository _homeRepository = homeRepository;

        public async Task<IActionResult> Index()
        {
            var result = await _homeRepository.GetTopDesigns();
            return View(result);
        }

        public IActionResult Unauthorized()
        {
            return View();
        }

        public IActionResult PageNotFound()
        {
            return View();
        }
        
        public IActionResult Unexpected()
        {
            return View();
        }
    }
}
