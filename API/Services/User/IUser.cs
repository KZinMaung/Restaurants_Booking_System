using Data.Dtos;
using Data.Dtos.User;
using Data.Models;

namespace API.Services.User
{
    public interface IUser
    {
        Task<object> Login(LoginRequest loginRequest);
        Task<bool> ChangePassword(ChangePasswordRequest request);
        Task<object> CreateUser(tbUser user);
        Task<tbUser> GetUserById(int id);
        Task<tbUser> UpdateUser(UpdateUserRequest request);

    }
}
