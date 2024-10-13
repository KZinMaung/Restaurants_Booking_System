using Core.Extension;
using Infra.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Data.Models;
using Data.Model;
using Infra.BlobStorage;
using Data.Dtos.User;
using Data.Constants;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

        public async Task<tbUser> GetUserById(int id)
        {
            return await _uow.userRepo.GetAll()
                .FirstOrDefaultAsync(a => a.Id == id && a.IsDeleted != true) ?? new tbUser();
        }

        public async Task<tbUser> UpdateUser(UpdateUserRequest request)
        {
            tbUser result = new tbUser();
            if (request.PhotoString != null)
            {
                request.Photo = await _azureBlobStorage.UploadFileToBlob(request.PhotoString, "jpeg");

            }
            if (request.Id > 0)
            {
                tbUser user = await GetUserById(request.Id);
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





        //login and register
        public async Task<object> Login(LoginRequest loginRequest)
        {

            var user = await _uow.userRepo.GetAll().Where(a => a.Email == loginRequest.Email && a.IsDeleted != true).FirstOrDefaultAsync() ?? new tbUser();
            if (user.Id != 0 && VerifyPassword(loginRequest.Password, user.Password))
            {
                var token = GenerateJwtToken(loginRequest.Email);
                return new
                {
                    Token = token
                };
            }
            else
            {
                throw new ArgumentException($"Unable to authenticate user {loginRequest.Email}");
            }

        }


        private string GenerateJwtToken(string email)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings["Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
            new Claim(ClaimTypes.Email, email)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        public async Task<Object> CreateUser(tbUser user)
        {

            tbUser userByEmail = await _uow.userRepo.GetAll().Where(a => a.Email == user.Email && a.IsDeleted != true).FirstOrDefaultAsync() ?? new tbUser();
            tbUser userByName = await _uow.userRepo.GetAll().Where(a => a.Name == user.Name && a.IsDeleted != true).FirstOrDefaultAsync() ?? new tbUser();
            if (userByEmail.Id != 0 || userByName.Id != 0)
            {
                throw new ArgumentException($"User with email {user.Email} or username {user.Name} already exists.");

            }
            else
            {
                tbUser entity = new tbUser
                {
                    Name = user.Name,
                    Email = user.Email,
                    Phone = user.Phone,
                    Password = HashPassword(user.Password),
                    CreatedAt = DateTime.Now,
                    Accesstime = DateTime.Now,
                    IsDeleted = false
                };
                var result = await _uow.userRepo.InsertReturnAsync(entity);
                return result;


            }

        }
    }
}
