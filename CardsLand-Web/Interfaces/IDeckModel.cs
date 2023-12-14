using CardsLand_Web.Entities;

namespace CardsLand_Web.Interfaces
{
    public interface IDeckModel
    {
        Task<ApiResponse<List<DeckEnt>>> GetAllUserDecks(long userId);
        Task<ApiResponse<DeckEnt>> GetSpecificDeck(long deckId);
        Task<ApiResponse<DeckEnt>> EditDeckValues(DeckEnt entity);
        Task<ApiResponse<List<CardEnt>>> GetCardsFromDeck(long deckId);
        Task<ApiResponse<DeckEnt>> CreateDeck(DeckEnt entity);
        Task<ApiResponse<CardDeckEnt>> AddCardToDeck(CardDeckEnt entity);
        Task<ApiResponse<DeckEnt>> DeleteDeck(long deckId);
        Task<ApiResponse<DeckEnt>> DeleteCardFromDeck(long deckId, string cardId);


    }
}
