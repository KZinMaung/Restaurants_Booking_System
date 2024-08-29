using Core.Extension;
using Infra.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Data.Models;
using Data.Model;
using Infra.BlobStorage;
using Data.Dtos.User;

namespace API.Services.User
{
    public class UserBase : IUser
    {
        private readonly BookingSystemDbContext _context;
        private readonly IConfiguration _configuration;
        private IAzureBlobStorage _azureBlobStorage;
        UnitOfWork _uow;

        public UserBase(BookingSystemDbContext context,IAzureBlobStorage azureBlobStorage, IConfiguration configuration)
        {
            this._context = context;
            this._uow = new UnitOfWork(_context);
            this._configuration = configuration;
            this._azureBlobStorage = azureBlobStorage;
        }

        public async Task<tbUser> GetById(int id)
        {
            return await _uow.userRepo.GetAll()
                .FirstOrDefaultAsync(a => a.Id == id && a.IsDeleted != true) ?? new tbUser();
        }

        public async Task<tbUser> EditProfile(EditProfileRequest request)
        {
            tbUser result = new tbUser();
            if (request.PhotoString != null)
            {
                request.Photo = await _azureBlobStorage.UploadFileToBlob(request.PhotoString, "jpeg");

            }
            if (request.Id > 0)
            {
                tbUser user = await GetById(request.Id);
                user.Name = request.Name;
                user.Email = request.Email;
                user.Phone = request.Phone;
                user.Photo = request.Photo;
                user.Accesstime = MyExtension.GetLocalTime();
                
                result = await _uow.userRepo.UpdateAsync(user);
            }
            
            return result;
        }

        public async Task<bool> ChangePassword(ChangePasswordRequest request)
        {

            var user = _uow.userRepo.GetById(request.UserID);
            if (user == null || !VerifyPassword(request.CurrentPassword, user.Password))
            {
                return false;
            }
            else
            {
                user.Password = HashPassword(request.NewPassword);
                user.Accesstime = MyExtension.GetLocalTime();
                tbUser updatedEntity = await _uow.userRepo.UpdateAsync(user) ?? new tbUser();

                if (updatedEntity.Id == 0)
                    return false;

                return true;
            }

        }

        private bool VerifyPassword(string inputPassword, string storedHash)
        {

            return BCrypt.Net.BCrypt.Verify(inputPassword, storedHash);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

    }
}
