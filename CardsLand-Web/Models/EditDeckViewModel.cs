using CardsLand_Web.Entities;

namespace CardsLand_Web.Models
{
    public class EditDeckViewModel
    {
        public DeckEnt? Deck { get; set; }
        public List<CardEnt>? CardList { get; set; }
        public CardEnt? Card { get; set; }
    }
}
