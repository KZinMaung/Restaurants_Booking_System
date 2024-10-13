using API.Services.User;
using Data.Dtos.User;
using Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    [ApiController]
    public class UserController : ControllerBase
    {
        IUser _iuser;
        public UserController(IUser iuser)
        {
            this._iuser = iuser;
        }

        [HttpPost("api/user/login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var result = await this._iuser.Login(loginRequest);
            return Ok(result);
        }

        [HttpPost("api/user/change_password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.CurrentPassword) || string.IsNullOrWhiteSpace(request.NewPassword))
            {
                return BadRequest("Invalid request.");
            }

            var result = await this._iuser.ChangePassword(request);

            if (!result)
            {
                return Unauthorized("Current password is incorrect or user does not exist.");
            }

            return Ok("Password changed successfully.");
        }



        [HttpPost("api/users")]
        public async Task<IActionResult> CreateUser(tbUser user)
        {
            var result = await this._iuser.CreateUser(user);
            return Ok(result);
        }

        [HttpGet("api/users/{id}")]
        public async Task<IActionResult> GetUserById(int id)

        {
            var result = await this._iuser.GetUserById(id);
            return Ok(result);
        }

        [HttpPut("api/users/{id}")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
        {
            var result = await this._iuser.UpdateUser(request);
            return Ok(result);
        }

       

    }
}
