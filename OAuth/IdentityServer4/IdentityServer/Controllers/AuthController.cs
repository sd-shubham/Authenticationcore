using IdentityModel;
using IdentityServer.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
            => (_signInManager, _userManager) = (signInManager, userManager);
        public IActionResult Login(String returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(loginModel.UserName, loginModel.Password, false, false);
            if (signInResult.Succeeded) return Redirect(loginModel.ReturnUrl);
            return View();
        }
        public IActionResult Register(String returnUrl)
        {
            return View(new RegisterViewModel { ReturnUrl = returnUrl });
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel loginModel)
        {
            if (!ModelState.IsValid) return View(loginModel);
            IdentityUser user = new IdentityUser(loginModel.UserName);
            var result = await _userManager.CreateAsync(user, loginModel.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return Redirect(loginModel.ReturnUrl);
            };
            return View();
        }
    }
}
