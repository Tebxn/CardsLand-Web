using Microsoft.AspNetCore.Mvc;

namespace CardsLand_Web.Controllers
{
    public class PokemonTcgController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
