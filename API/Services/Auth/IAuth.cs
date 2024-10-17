using Data.Dtos;

namespace API.Services.Auth
{
    public interface IAuth
    {
        Task<AuthenticaticationData> Login(LoginRequest loginRequest);

    }
}
