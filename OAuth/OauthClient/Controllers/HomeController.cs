using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OauthClient.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
         => View();
        [Authorize]
        public ActionResult Secure()
         => View();
    }
}