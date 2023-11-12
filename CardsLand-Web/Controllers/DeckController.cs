using Microsoft.AspNetCore.Mvc;

namespace CardsLand_Web.Controllers
{
    public class DeckController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateDeck() 
        {
            return View("CreateDeck");
        }
    }
}
