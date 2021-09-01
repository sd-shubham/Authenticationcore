using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Net.Http;
using IdentityModel.Client;
namespace ApiTwo.Controllers
{
    [ApiController]
    [Route("api/client")]
    public class HomeController : ControllerBase
    {
        private readonly IHttpClientFactory _clientfactory;
        public HomeController(IHttpClientFactory clientFactory)
        {
            _clientfactory = clientFactory;
        }
        [HttpGet("ind")]
        public async Task<IActionResult> Index()
        {
            // retrieve access token

            var serverClient = _clientfactory.CreateClient();
            var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync("https://localhost:5001");

            var tokenResponse = await serverClient.RequestClientCredentialsTokenAsync(
                  new ClientCredentialsTokenRequest
                  {
                      Address = discoveryDocument.TokenEndpoint,
                      ClientId = "client_id",
                      ClientSecret = "client_secret",
                      Scope = "ApiOne.fullAccess"

                  });
            var apiClient = _clientfactory.CreateClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);
            var response = await apiClient.GetAsync("https://localhost:5003/api"); // apione url
            var content = await response.Content.ReadAsStringAsync();
            return Ok(new
            {
                access_token = tokenResponse.AccessToken,
                message = content
            }); //
        }
    }
}