﻿using CardsLand_Api.Interfaces;
using CardsLand_Web.Entities;
using PokemonTcgSdk.Standard.Features.FilterBuilder.Pokemon;
using PokemonTcgSdk.Standard.Infrastructure.HttpClients;
using PokemonTcgSdk.Standard.Infrastructure.HttpClients.Base;
using PokemonTcgSdk.Standard.Infrastructure.HttpClients.Cards;

namespace CardsLand_Api.Implementations
{
    public class PokemonTcg : IPokemonTcg
    {
        readonly PokemonApiClient pokeClient = new PokemonApiClient("6c488163-f020-49f7-829c-3bdb02c474f7");

        public async Task<ApiResourceList<Card>> GetAllCards()
        {
            var filter = PokemonFilterBuilder.CreatePokemonFilter()
            .AddSetName("Base");
            var cards = await pokeClient.GetApiResourceAsync<Card>(filter);
            return cards;
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
                listCards.Add(cardEnt.ReturnCard(x.Id, x.Name, x.Images.Small.ToString()));
            }

            return listCards;
        }
    }
}