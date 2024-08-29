using API.Services.Auth;
using Data.Dtos.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [ApiController]
    public class AuthController : ControllerBase
    {
        IAuth _iauth;
        public AuthController(IAuth iauth)
        {
            this._iauth = iauth;
        }

        [HttpPost("api/auth/login")]
        public async Task<IActionResult> Login(LoginRequestDto loginRequest)
        {
            var result = await this._iauth.Login(loginRequest);
            return Ok(result);
        }

        [HttpPost("api/auth/register")]
        public async Task<IActionResult> Register(RegisterRequestDto registerRequest)
        {
            var result = await this._iauth.Register(registerRequest);
            return Ok(result);
        }
    }
}
