using CardsLand_Web.Entities;
using CardsLand_Web.Implementations;

namespace CardsLand_Web.Interfaces
{
    public interface IUserModel
    {

        Task<ApiResponse<UserEnt>> Login(UserEnt entity);
        Task<ApiResponse<UserEnt>> RegisterAccount(UserEnt entity);
        Task<ApiResponse<UserEnt>> GetSpecificUserFromToken();
        Task<ApiResponse<List<UserEnt>>> GetAllUsers();
        Task<ApiResponse<UserEnt>> GetSpecificUser(long userId);
        Task<ApiResponse<UserEnt>> EditSpecificUser(UserEnt entity);
        Task<ApiResponse<UserEnt>> PwdRecovery(UserEnt entity);
        Task<ApiResponse<UserEnt>> UpdateNewPassword(UserEnt entity);
        Task<ApiResponse<UserEnt>> ActivateAccount(UserEnt entity);
        Task<ApiResponse<UserEnt>> UpdateUserState(UserEnt entity);

    }
}
