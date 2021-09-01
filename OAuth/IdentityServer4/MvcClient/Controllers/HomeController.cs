using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace MvcClient.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Secret()
        {
            // get token from request

            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var idToken = await HttpContext.GetTokenAsync("id_token");
            var refreseToken = await HttpContext.GetTokenAsync("refresh_token");
            var access_token = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            var id_token = new JwtSecurityTokenHandler().ReadJwtToken(idToken);
            var cliams = User.Claims;
            return View();
        }
    }
}
