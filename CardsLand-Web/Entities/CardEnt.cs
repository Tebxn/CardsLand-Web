using Newtonsoft.Json.Linq;
using PokemonTcgSdk.Standard.Infrastructure.HttpClients.Cards.Models;
using System.Data;

namespace CardsLand_Web.Entities
{
    public class CardEnt
    {
        public string Card_Id { get; set; } = string.Empty; //example: 
        public string Card_Name { get; set; } = string.Empty;
        public string Card_Image_Url { get; set; } = string.Empty;
        public List<string> Subtypes { get; set; }
        public List<string> Types { get; set; }
        public List<Ability> Abilities { get; set; }
        public int? Card_Quantity { get; set; }


        public CardEnt ReturnCard(string id, string name, string image, List<string> subtypes, List<string> types, List<Ability> abilities)
        {
            return new CardEnt
            {
                Card_Id = id,
                Card_Name = name,
                Card_Image_Url = image,
                Subtypes = subtypes,
                Types = types,
                Abilities = abilities
                
            };
        }
    }



}




