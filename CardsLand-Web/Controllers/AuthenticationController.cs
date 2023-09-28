using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CardsLand_Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(ILogger<AuthenticationController> logger)
        {
            _logger = logger;
        }
        public IActionResult AuthLogin()
        {
            return View();
        }
        public IActionResult AuthRegister()
        {
            return View();
        }
        public IActionResult AuthRecoverPW()
        {
            return View();
        }
        public IActionResult Auth404()
        {
            return View();
        }
        public IActionResult Auth500()
        {
            return View();
        }
    }
}
