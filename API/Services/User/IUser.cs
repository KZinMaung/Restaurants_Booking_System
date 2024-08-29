using Data.Dtos;
using Data.Dtos.User;
using Data.Models;

namespace API.Services.User
{
    public interface IUser
    {
        Task<tbUser> GetById(int id);
        Task<tbUser> EditProfile(EditProfileRequest request);
        Task<bool> ChangePassword(ChangePasswordRequest request);

    }
}
