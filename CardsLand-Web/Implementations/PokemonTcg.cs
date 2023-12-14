using CardsLand_Api.Interfaces;
using CardsLand_Web.Entities;
using PokemonTcgSdk.Standard.Features.FilterBuilder.Pokemon;
using PokemonTcgSdk.Standard.Infrastructure.HttpClients;
using PokemonTcgSdk.Standard.Infrastructure.HttpClients.Base;
using PokemonTcgSdk.Standard.Infrastructure.HttpClients.Cards;
using PokemonTcgSdk.Standard.Infrastructure.HttpClients.Cards.Models;

namespace CardsLand_Api.Implementations
{
    public class PokemonTcg : IPokemonTcg
    {
        readonly PokemonApiClient pokeClient = new PokemonApiClient("6c488163-f020-49f7-829c-3bdb02c474f7");

        public async Task<List<CardEnt>> GetAllCards()
        {
            List<CardEnt> listCards = new List<CardEnt>();
            CardEnt cardEnt = new CardEnt();

            var filter = PokemonFilterBuilder.CreatePokemonFilter().AddSetName("Base");
            var cards = await pokeClient.GetApiResourceAsync<Card>(filter);

            foreach (var x in cards.Results)
            {
                List<string> subtypes = x.Subtypes != null ? x.Subtypes.ToList() : new List<string>();
                List<string> types = x.Types != null ? x.Types.ToList() : new List<string>();

                // Obtener habilidades
                List<Ability> abilities = x.Abilities != null ? x.Abilities.ToList() : new List<Ability>();

                listCards.Add(cardEnt.ReturnCard(x.Id, x.Name, x.Images.Small.ToString(), subtypes, types, abilities));
            }

            return listCards;
        }

        public async Task<List<CardEnt>> GetSpecificCardbyName(string pokemonCardName)
        {
            List<CardEnt> listCards = new List<CardEnt>();
            CardEnt cardEnt = new CardEnt();

            var filter = PokemonFilterBuilder.CreatePokemonFilter()
            .AddName(pokemonCardName)
            .AddSetName("Base");
            var cards = await pokeClient.GetApiResourceAsync<Card>(filter);
            //cardEnt.ReturnCard(cards);

            foreach (var x in cards.Results)
            {
                List<string> subtypes = x.Subtypes != null ? x.Subtypes.ToList() : new List<string>();
                List<string> types = x.Types != null ? x.Types.ToList() : new List<string>();

                // Obtener habilidades
                List<Ability> abilities = x.Abilities != null ? x.Abilities.ToList() : new List<Ability>();

                listCards.Add(cardEnt.ReturnCard(x.Id, x.Name, x.Images.Small.ToString(), subtypes, types, abilities));
            }

            return listCards;
        }

        public async Task<CardEnt> GetSpecificCardbyId(string pokemonCardId)
        {
            var filter = PokemonFilterBuilder.CreatePokemonFilter()
                .AddId(pokemonCardId)
                .AddSetName("Base");

            var cards = await pokeClient.GetApiResourceAsync<Card>(filter);

            if (cards.Results.Count > 0)
            {
                var firstCard = cards.Results[0];
                return new CardEnt
                {
                    Id = firstCard.Id,
                    Name = firstCard.Name,
                    ImageUrl = firstCard.Images?.Small?.ToString()
                };
            }

            return null;
        }

    }
}
