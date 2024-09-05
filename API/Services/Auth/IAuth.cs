using Data.Dtos.Auth;

namespace API.Services.Auth
{
    public interface IAuth
    {
        Task<object> Login(LoginRequestDto loginRequest);
        Task<object> Register(RegisterRequestDto registerRequest);
    }
}
