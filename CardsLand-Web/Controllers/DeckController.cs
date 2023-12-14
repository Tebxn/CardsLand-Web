using CardsLand_Api.Interfaces;
using CardsLand_Web.Entities;
using CardsLand_Web.Interfaces;
using CardsLand_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace CardsLand_Web.Controllers
{
    public class DeckController : Controller
    {
        private readonly IDeckModel _deckModel;

        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly IPokemonTcg _pokeTcgModel;


        public DeckController(IDeckModel deckModel, IHttpContextAccessor httpContextAccessor, IPokemonTcg pokeTcgModel)
        {
            _deckModel = deckModel;
            _HttpContextAccessor = httpContextAccessor;
            _pokeTcgModel = pokeTcgModel;
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
                EditDeckViewModel editDeckViewModel = new EditDeckViewModel();

                var deckData = await _deckModel.GetSpecificDeck(deckId);
                var listDeckData = await _deckModel.GetCardsFromDeck(deckId);
                
                editDeckViewModel.Deck = deckData.Data;
                editDeckViewModel.CardList = listDeckData.Data;

                if (deckData.Success && listDeckData.Success)
                {
                    if (editDeckViewModel != null)
                    {
                        return View("EditDeck", editDeckViewModel);
                    }
                    else
                    {
                        ViewBag.MensajePantalla = deckData.ErrorMessage;
                        return View("Mis Mazos");
                    }
                }
                else
                {
                    ViewBag.MensajePantalla = deckData.ErrorMessage;
                    return View();
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditDeckValues(EditDeckViewModel entity)
        {
            try
            {
                var response = await _deckModel.EditDeckValues(entity.Deck);

                if (response.Success)
                {
                    return RedirectToAction("EditDeck", new { deckId = entity.Deck.Deck_Id });
                }
                else
                {
                    TempData["MensajePantalla"] = "Error editando el mazo";
                }
            }
            catch (Exception ex)
            {
                TempData["MensajePantalla"] = "Error al cargar los datos";
            }

            return RedirectToAction("EditDeck", new { deckId = entity.Deck.Deck_Id });
        }



        [HttpGet]
        public IActionResult CreateDeck()
        {
            return View("CreateDeck");
        }

        [HttpPost]
        public async Task<IActionResult> CreateDeck(DeckEnt entity)
        {
            try
            {
                var card = await _pokeTcgModel.GetSpecificCardbyName(entity.Deck_Background_Image);
                foreach (var item in card)
                {
                    entity.Deck_Background_Image = item.Card_Image_Url;
                }

                var apiResponse = await _deckModel.CreateDeck(entity);

                if (apiResponse.Success)
                {
                    return RedirectToAction("MisMazos", "Deck");
                }
                else
                {
                    ViewBag.MensajePantalla = "No se pudo registrar la cuenta";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.MensajePantalla = "Error al cargar los datos";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCardToDeck(EditDeckViewModel entity)
        {
            try
            {
                var cards = await _pokeTcgModel.GetSpecificCardbyName(entity.Card.Card_Name);
                string cardId = cards.FirstOrDefault()?.Card_Id ?? "";  // Utiliza FirstOrDefault para obtener el primer elemento

                CardDeckEnt cardDeckEnt = new CardDeckEnt();
                cardDeckEnt.CardId = cardId;
                cardDeckEnt.DeckId = entity.Deck.Deck_Id;

                if (String.IsNullOrEmpty(cardId))
                {
                    return RedirectToAction("EditDeck", "Deck", new { deckId = entity.Deck.Deck_Id });
                }
                var apiResponse = await _deckModel.AddCardToDeck(cardDeckEnt);

                if (apiResponse.Success)
                {
                    return RedirectToAction("EditDeck", "Deck", new { deckId = entity.Deck.Deck_Id });
                }
                else
                {
                    ViewBag.MensajePantalla = "No se pudo registrar la cuenta";
                    return RedirectToAction("EditDeck", "Deck", new { deckId = entity.Deck.Deck_Id });
                }
            }
            catch (Exception ex)
            {
                ViewBag.MensajePantalla = "Error al cargar los datos";
                return RedirectToAction("EditDeck", "Deck", new { deckId = entity.Deck.Deck_Id });
            }
        }

    }
}
