using Newtonsoft.Json.Linq;
using PokemonTcgSdk.Standard.Infrastructure.HttpClients.Cards.Models;
using System.Data;

namespace CardsLand_Web.Entities
{
    public class CardEnt
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public List<string> Subtypes { get; set; }
        public List<string> Types { get; set; }
        public List<Ability> Abilities { get; set; }


        public CardEnt ReturnCard(string id, string name, string image, List<string> subtypes, List<string> types, List<Ability> abilities)
        {
            return new CardEnt
            {
                Id = id,
                Name = name,
                ImageUrl = image,
                Subtypes = subtypes,
                Types = types,
                Abilities = abilities
            };
        }
    }



}




