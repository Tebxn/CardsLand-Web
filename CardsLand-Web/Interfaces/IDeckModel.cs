using CardsLand_Web.Entities;

namespace CardsLand_Web.Interfaces
{
    public interface IDeckModel
    {
        Task<ApiResponse<List<DeckEnt>>> GetAllUserDecks(long userId);
    }
}
