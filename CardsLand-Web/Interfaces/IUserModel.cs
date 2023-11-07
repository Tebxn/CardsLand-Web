using CardsLand_Web.Entities;

namespace CardsLand_Web.Interfaces
{
    public interface IUserModel
    {

        Task<ApiResponse<UserEnt>> Login(UserEnt entity);
        Task<ApiResponse<UserEnt>> RegisterAccount(UserEnt entity);
    }
}
