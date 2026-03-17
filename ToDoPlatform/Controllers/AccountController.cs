using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ToDoPlatform.Services;
using ToDoPlatform.ViewModels;

namespace ToDoPlatform.Controllers;

    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
    private readonly IUserService _userService;

    public AccountController(
            ILogger<AccountController> logger, 
            IUserService  userService
        )
        {
            _logger = logger;
            _userService = userService;
        }

        public IActionResult Login(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

                var model = new LoginVM
                {
                    ReturnUrl = returnUrl ?? Url.Content("~/")
                };           
            return View(model);
        }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginVM login)
    {
        if (ModelState.IsValid)
        {
            var result = await _userService.Login(login);
            if (result.Succeeded)
                return LocalRedirect(login.ReturnUrl);
            if (result.IsLockedOut)
                return RedirectToAction("Lockout");
            if (result.IsNotAllowed)
                return RedirectToAction("AccessDenied");
            ModelState.AddModelError ("", "Usuário e/ou Senha Inválidos");    
        }
        return View(login);
    }


        public IActionResult Logout()
        {
            return View("Login");
        }

        public IActionResult Register()
        {
            return View();
        }


        public IActionResult Profile()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
