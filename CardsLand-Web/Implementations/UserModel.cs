using CardsLand_Web.Entities;
using CardsLand_Web.Interfaces;

namespace CardsLand_Web.Implementations
{
    public class UserModel : IUserModel
    {

        Task<ApiResponse<UserEnt>> IUserModel.Login(UserEnt entity)
        {
            throw new NotImplementedException();
        }

        Task<ApiResponse<UserEnt>> IUserModel.RegisterAccount(UserEnt entity)
        {
            throw new NotImplementedException();
        }
    }
}
