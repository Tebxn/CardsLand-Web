using CardsLand_Web.Entities;
using PokemonTcgSdk.Standard.Infrastructure.HttpClients.Base;
using PokemonTcgSdk.Standard.Infrastructure.HttpClients.Cards;

namespace CardsLand_Api.Interfaces
{
    public interface IPokemonTcg
    {
        Task<ApiResourceList<Card>> GetAllCards();
        Task<List<CardEnt>> GetSpecificCardbyName(string pokemonCardName);
    }
}
