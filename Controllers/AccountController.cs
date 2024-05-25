using FashionWebsite.Models;
using FashionWebsite.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FashionWebsite.Controllers
{
    public class AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IHttpContextAccessor contextAccessor) : Controller
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly SignInManager<AppUser> _signInManager = signInManager;
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid Input";
                return View(loginViewModel);
            }

            var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);

            if (user != null)
            {
                var password = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);

                if (password)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        var returnUrl = _contextAccessor.HttpContext.Request.Query["returnUrl"].ToString();

                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }

                        return RedirectToAction("Index", "Home");
                    }
                }
                TempData["Error"] = "Wrong credentials";
                return View(loginViewModel);
            }

            TempData["Error"] = "Wrong credentials";
            return View(loginViewModel);
        }

        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid) return View(registerViewModel);

            var user = await _userManager.FindByEmailAsync(registerViewModel.Email);
            if (user != null)
            {
                TempData["Errors"] = "This email address is already in use";
                return View(registerViewModel);
            }

            var newUser = new AppUser()
            {
                Email = registerViewModel.Email,
                FirstName = registerViewModel.FirstName,
                LastName = registerViewModel.LastName,
                UserName = registerViewModel.Username,
                PhoneNumber = registerViewModel.PhoneNumber,
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);

            if (newUserResponse.Succeeded)
            {
                if(registerViewModel.AsFashionista)
                    await _userManager.AddToRoleAsync(newUser, "Fashionista");
                else
                    await _userManager.AddToRoleAsync(newUser, "Customer");
            }
            else
            {
                foreach (var error in newUserResponse.Errors)
                {
                    var errorMessages = newUserResponse.Errors.Select(error => error.Description).ToArray();
                    TempData["Errors"] = errorMessages;
                }
                return View(registerViewModel);
            }

            return RedirectToAction("Login", "Account");
        }  

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
