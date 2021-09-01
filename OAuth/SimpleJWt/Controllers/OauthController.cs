using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SimpleJWt.Controllers
{
    public class Oauth: Controller
    {
        [HttpGet]
        public IActionResult Authorization(
            string response_type,
            string client_id,
            string redirect_uri,
            string scope,
            string state
            )
        {
            var query = new QueryBuilder
            {
                { "redirectUri", redirect_uri },
                { "state", state }
            };
            return View(model: query.ToString());

        }
        [HttpPost]
        public IActionResult Authorization(
            string userName,
            string redirectUri,
            string state
            )
        {
            const string code = "bababab";
            var query = new QueryBuilder
            {
                {"code",code },
                {"state", state }
            };
            return Redirect($"{redirectUri}{query}");
        }
        public  IActionResult Token(
            string grant_type,
            string code,
            string redirect_uri,
            string client_id
            )
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
            var signInCredentials = new SigningCredentials(key, algorithm);
            var token = new JwtSecurityToken(
                issuer: Constant.Issuers,
                audience: Constant.Audiance,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(2),
                signingCredentials: signInCredentials
                );
            // step 3 convert token to json string
            var access_token = new JwtSecurityTokenHandler().WriteToken(token);
            var responseObject = new
            {
                access_token,
                token_type = "Bearer",
                raw_cliam = "OauthDmeo"
            };
            //var jsonresponseInBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(responseObject));
            //await Response.Body.WriteAsync(jsonresponseInBytes,0,jsonresponseInBytes.Length);
            return Ok(responseObject);
        }
    }
}