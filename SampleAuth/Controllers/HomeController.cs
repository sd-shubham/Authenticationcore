using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using SampleAuth.CustomePolicyProvider;

namespace SampleAuth.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        // [Authorize]
        // [Authorize(Policy = "Claim.DOB")]
        // [Authorize(Roles = "Admin1")]
        [SecurityLevel(5)]
        public IActionResult Secret()
        {
            return View();
        }
        public IActionResult Auth()
        {
            var myClaims = new List<Claim>(){
                new Claim(ClaimTypes.Name,"Test"),
                new Claim(ClaimTypes.Email,"Test@Test.com"),
                new Claim(ClaimTypes.Role,"Admin1"),
                new Claim("userId","1"),
                new Claim(DynamicPolicies.SecurityLevel,"5")
            };
            var uIdClaims = new List<Claim>(){
                new Claim(ClaimTypes.Email,"Test@Test.com"),
                new Claim(ClaimTypes.DateOfBirth,"1/1/2021"),
                new Claim("uId","A123")
            };
            var userIdentity = new ClaimsIdentity(myClaims, "simple auth");
            var useruniversalIdentity = new ClaimsIdentity(uIdClaims, "Universal Auth");
            var userPrinciple = new ClaimsPrincipal(new[] { userIdentity, useruniversalIdentity });
            HttpContext.SignInAsync(userPrinciple);
            return RedirectToAction(nameof(Index));
        }
    }
    public class SecurityLevelAttribute : AuthorizeAttribute
    {
        public SecurityLevelAttribute(int level)
        {
            //{type}.{value}
            Policy = $"{DynamicPolicies.SecurityLevel}.{level}";
        }
    }
}