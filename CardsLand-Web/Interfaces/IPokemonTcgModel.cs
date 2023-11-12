using CardsLand_Web.Entities;

namespace CardsLand_Web.Interfaces
{
    public interface IPokemonTcgModel
    {
        Task<ApiResponse<ApiResourceList<CardEnt>>> GetAllCards();
    }
}
