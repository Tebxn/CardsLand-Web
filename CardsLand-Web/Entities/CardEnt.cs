namespace CardsLand_Web.Entities
{
    public class CardEnt
    {
        public string CardId { get; set; } = string.Empty; //example: 
        public string CardName { get; set; } = string.Empty;
        public string CardImageUrl { get; set; } = string.Empty;

        //Propiedades de las cartas consumidas por el API
        public string? Id { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;

        public Uri Small { get; set; }

        public Uri Large { get; set; }
    }


}
