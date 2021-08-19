using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
namespace SimpleJWt.Controllers
{
    public class HomeController: Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult Secure()
        {
            return View();
        }
        public ActionResult Auth()
        {
            // step 1: step claim
            var claims = new[]
            {
                //since here we are not using db or identity so we hardcoded sub to 1
                // jsut remember 1 is "some_id"
                new Claim(JwtRegisteredClaimNames.Sub,"1"),
                new Claim("myCliam","jwt_auth")
            };
            // step 2:  create or generate token
            var secretBytes = Encoding.UTF8.GetBytes(Constant.SecurityKey);
            var key = new SymmetricSecurityKey(secretBytes);
            var algorithm = SecurityAlgorithms.HmacSha256;
            var signInCredentials = new SigningCredentials(key,algorithm);
            var token = new JwtSecurityToken(
                issuer:Constant.Issuers,
                audience:Constant.Audiance,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(2),
                signingCredentials: signInCredentials
                );
            // step 3 convert token to json string
            var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new { access_token = tokenJson });
        }
    }
    public static class Constant
    {
        public const string Audiance = "https://localhost:5001/";
        public const string Issuers = "https://localhost:5001/";
        public const string SecurityKey = "My_Test_Key_For_Token_Generation";
    }
}
