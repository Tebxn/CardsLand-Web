using Newtonsoft.Json.Linq;
using System.Data;

namespace CardsLand_Web.Entities
{
    public class CardEnt
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }

        public CardEnt ReturnCard(string id, string name, string image)
        {
            return new CardEnt
            {
                Id = id,
                Name = name,
                ImageUrl = image
            };
        }
    }


}




