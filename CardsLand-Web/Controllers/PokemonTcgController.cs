using CardsLand_Web.Entities;
using CardsLand_Web.Interfaces;
using CardsLand_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

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
        public async Task<IActionResult> GetSpecificCardByName(string pokemonCardName)
        {
            try
            {
                var apiResponse = await _pokemonModel.GetSpecificCardByName(pokemonCardName);

                if (apiResponse.Success)
                {
                    var listCards = apiResponse.Data;
                    return View("GetSpecificCardByName", listCards);
                }
                else
                {
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return View("Error");
            }
        }



    }
}
