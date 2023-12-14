using CardsLand_Web.Entities;

namespace CardsLand_Web.Interfaces
{
    public interface IError
    {
        Task<ApiResponse<ErrorEnt>> AddError(ErrorEnt entity);
    }
}
