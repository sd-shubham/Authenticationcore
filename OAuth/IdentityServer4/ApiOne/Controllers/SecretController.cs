
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ApiOne.Controllers
{
    [ApiController]
    [Route("api")]
    public class SecretController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public string Secret() => "my secret";
    }
}