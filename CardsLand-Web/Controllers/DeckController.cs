using CardsLand_Web.Interfaces;
using CardsLand_Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CardsLand_Web.Controllers
{
    public class DeckController : Controller
    {
        private readonly IDeckModel _deckModel;

        private readonly IHttpContextAccessor _HttpContextAccessor;


        public DeckController(IDeckModel deckModel, IHttpContextAccessor httpContextAccessor)
        {
            _deckModel = deckModel;
            _HttpContextAccessor = httpContextAccessor;

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> MisMazos()
        {
            try
            {
                long userId = long.Parse(_HttpContextAccessor.HttpContext.Session.GetString("UserId"));
                var apiResponse = await _deckModel.GetAllUserDecks(userId);

                if (apiResponse.Success)
                {
                    var data = apiResponse.Data;
                    if (data != null)
                    {
                        return View(data);
                    }
                    else
                    {
                        ViewBag.MensajePantalla = "Ha ocurrido un error para mostrar tus mazos";
                        return View();
                    }
                }
                else
                {
                    ViewBag.MensajePantalla = "No se logro conexion con el servidor";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.MensajePantalla = "Error al cargar los datos";
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditDeck(long deckId)
        {
            try
            {
                var apiResponse = await _deckModel.GetSpecificDeck(deckId);
                var cardsFromDeck = await _deckModel.GetCardsFromDeck(deckId);
                ViewBag.cards = cardsFromDeck.Data;
                if (apiResponse.Success)
                {
                    var data = apiResponse.Data;
                    if (data != null)
                    {

                        return View("EditDeck", data);
                    }
                    else
                    {
                        ViewBag.MensajePantalla = apiResponse.ErrorMessage;
                        return View("Mis Mazos");
                    }
                }
                else
                {
                    ViewBag.MensajePantalla = apiResponse.ErrorMessage;
                    return View();
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult CreateDeck()
        {
            return View("CreateDeck");
        }
    }
}
