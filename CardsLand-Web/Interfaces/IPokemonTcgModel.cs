using CardsLand_Web.Entities;
using Microsoft.AspNetCore.Mvc;
using PokemonTcgSdk.Standard.Infrastructure.HttpClients.Base;
using PokemonTcgSdk.Standard.Infrastructure.HttpClients.Cards;
using System.ComponentModel;

namespace CardsLand_Web.Interfaces
{
    public interface IPokemonTcgModel
    {
        Task<ApiResourceList<Card>> GetSpecificCardByName(string pokemonCardName);



    }
}
