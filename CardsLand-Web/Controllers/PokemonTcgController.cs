using CardsLand_Api.Interfaces;
using CardsLand_Web.Entities;
using CardsLand_Web.Interfaces;
using CardsLand_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace CardsLand_Web.Controllers
{
    public class PokemonTcgController : Controller
    {

        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly IPokemonTcg _PokemonTcg;

        public PokemonTcgController(IPokemonTcg pokemonTcg, IHttpContextAccessor httpContextAccessor)
        {
            _PokemonTcg = pokemonTcg;
            _HttpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetSpecificCardByName(string pokemonCardName)
        {
            try
            {
                var apiPokemon = await _PokemonTcg.GetSpecificCardbyName(pokemonCardName);

                var listCards = apiPokemon.Results;

                return View("GetSpecificCardByName", listCards);
                //List<PokemonTcgSdk.Standard.Infrastructure.HttpClients.Cards.Card>
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }



    }
}
