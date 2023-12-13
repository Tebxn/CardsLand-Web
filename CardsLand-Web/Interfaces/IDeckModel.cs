using CardsLand_Web.Entities;

namespace CardsLand_Web.Interfaces
{
    public interface IDeckModel
    {
        Task<ApiResponse<List<DeckEnt>>> GetAllUserDecks(long userId);
        Task<ApiResponse<DeckEnt>> GetSpecificDeck(long deckId);

        Task<ApiResponse<DeckEnt>> EditDeck(long deckId);
        Task<ApiResponse<List<CardEnt>>> GetCardsFromDeck(long deckId);
    }
}
