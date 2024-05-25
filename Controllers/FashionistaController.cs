using FashionWebsite.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FashionWebsite.Controllers
{
    public class FashionistaController(IFashionistaRepository fashionistaRepository) : Controller
    {
        private readonly IFashionistaRepository _fashionistaRepository = fashionistaRepository;

        public async Task<IActionResult> Index()
        {
            var result = await _fashionistaRepository.GetAllFashionista();

            if (result == null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }

            return View(result);
        }

        public async Task<IActionResult> Page(string userId)
        {
            var result = await _fashionistaRepository.GetFashionistaPage(userId);

            if(result == null)
            {
                return RedirectToAction("PageNotFound","Home");
            }

            return View(result);
        }
    }
}
