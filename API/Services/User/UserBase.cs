using Core.Extension;
using Infra.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Data.Models;
using Data.Model;
using Infra.BlobStorage;
using Data.Dtos.User;
using Data.Constants;

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


        //Remains : to check and control duplication
        public async Task<string> BookRestaurant(tbBooking booking)
        {
            
            booking.Accesstime = MyExtension.GetLocalTime();
            booking.CreatedAt = MyExtension.GetLocalTime();
            booking.Status = BookingStatus.Booked;
            tbBooking result = await _uow.bookingRepo.InsertReturnAsync(booking);


            foreach(var item in booking.TableIds)
            {
                tbBookingTable bookingTable = new tbBookingTable();
                bookingTable.BookingId = result.Id;
                bookingTable.TableId = item;
                bookingTable.Accesstime = MyExtension.GetLocalTime();
                bookingTable.CreatedAt = MyExtension.GetLocalTime();
                tbBookingTable entity=await _uow.bookingTableRepo.InsertReturnAsync(bookingTable);
                if(entity == null)
                {
                    //rollback => delete booking record 
                    return ResponseStatus.Fail; 
                   
                }

            }

            return result != null ? ResponseStatus.Success : ResponseStatus.Fail;

        }
    }
}
