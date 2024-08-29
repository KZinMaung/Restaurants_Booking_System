using Data.Dtos.Auth;
using Data.Model;
using Data.Models;
using Infra.UnitOfWork;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace API.Services.Auth
{
    public class AuthBase : IAuth
    {
        private readonly BookingSystemDbContext _context;
        private readonly IConfiguration _configuration;
        UnitOfWork _uow;
        
        public AuthBase(BookingSystemDbContext context, IConfiguration configuration)
        {
            this._context = context;
            this._uow = new UnitOfWork(_context);
            this._configuration = configuration;
        }


        private bool VerifyPassword(string inputPassword, string storedHash)
        {

            return BCrypt.Net.BCrypt.Verify(inputPassword, storedHash);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public async Task<object> Login(LoginRequestDto loginRequest)
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



        public async Task<Object> Register(RegisterRequestDto registerRequest)
        {
            
            tbUser userByEmail = await _uow.userRepo.GetAll().Where(a => a.Email == registerRequest.Email && a.IsDeleted != true).FirstOrDefaultAsync() ?? new tbUser();
            tbUser userByName = await _uow.userRepo.GetAll().Where(a => a.Name == registerRequest.Name && a.IsDeleted != true).FirstOrDefaultAsync() ?? new tbUser();
            if (userByEmail.Id != 0 || userByName.Id != 0)
            {
                throw new ArgumentException($"User with email {registerRequest.Email} or username {registerRequest.Name} already exists.");


            }
            else
            {
                tbUser entity = new tbUser
                {
                    Name = registerRequest.Name,
                    Email = registerRequest.Email,
                    Phone = registerRequest.Phone,
                    Password = HashPassword(registerRequest.Password),
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

