using CardsLand_Web.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace CardsLand_Web.Interfaces
{
    public interface IPokemonTcgModel
    {
        Task<ApiResponse<List<CardEnt>>> GetSpecificCardByName(string pokemonCardName);

    }
}
