using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BookBox.ViewModels;
using Microsoft.Extensions.Logging;
using BookBox.Models;

namespace BookBox.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger _logger;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
                return View(loginViewModel);

            var user = await _userManager.FindByNameAsync(loginViewModel.UserName);

            if (user != null)
            {
                _logger.LogInformation(LoggingEvents.GetItem,
                        "User {USER} found", loginViewModel.UserName);

                var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                if (result.Succeeded)
                {
                    _logger.LogInformation(LoggingEvents.GetItem,
                        "User {USER} logged in", loginViewModel.UserName);

                    return RedirectToAction("Index", "Home");
                }
            }

            _logger.LogWarning(LoggingEvents.GetItemNotFound,
                        "Username {USER}/password not found",
                        loginViewModel.UserName);

            ModelState.AddModelError("", "Username/password not found");
            return View(loginViewModel);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser() { UserName = loginViewModel.UserName };
                var result = await _userManager.CreateAsync(user, loginViewModel.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation(LoggingEvents.InsertItem,
                        "User {USER} created", loginViewModel.UserName);

                    IdentityResult roleResult = await _userManager.AddToRoleAsync(user, "User");

                    if (roleResult.Succeeded)
                    {
                        _logger.LogInformation(LoggingEvents.InsertItem,
                        "User {USER} assigned to User role", loginViewModel.UserName);

                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        _logger.LogWarning(LoggingEvents.CreateUpdateItemFailed,
                        "Error while creating user {USER}: {ERROR}",
                        loginViewModel.UserName, error.Description);

                        ModelState.AddModelError("", error.Description);
                    }   
                }

            }
            return View(loginViewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation(LoggingEvents.Logout,
                        "{USER} logout", User.Identity.Name);

            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}