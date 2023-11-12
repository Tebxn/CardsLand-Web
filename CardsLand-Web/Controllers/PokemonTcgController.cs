using CardsLand_Web.Entities;
using CardsLand_Web.Interfaces;
using CardsLand_Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CardsLand_Web.Controllers
{
    public class PokemonTcgController : Controller
    {

        private readonly IPokemonTcgModel _pokemonModel;
        private readonly IHttpContextAccessor _HttpContextAccessor;

        public PokemonTcgController(IPokemonTcgModel pokemonModel, IHttpContextAccessor httpContextAccessor)
        {
            _pokemonModel = pokemonModel;
            _HttpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCards()
        {
            try
            {
                var apiResponse = await _pokemonModel.GetAllCards();
                var listcards = apiResponse.Data?.Results;
                return View(listcards ?? new List<CardEnt>());
            }
            catch (Exception)
            {
                List<CardEnt> errors = new List<CardEnt>();
                return View(errors);
            }
        }

    }
}
