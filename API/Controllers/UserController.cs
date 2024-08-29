using API.Services.User;
using Data.Dtos.User;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

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


        
        [HttpGet("api/user/view_profile")]
        public async Task<IActionResult> ViewProfile(int id)

        {
            var result = await this._iuser.GetById(id);
            return Ok(result);
        }

        [HttpPost("api/user/edit_profile")]
        public async Task<IActionResult> EditProfile([FromBody] EditProfileRequest request)
        {
            var result = await this._iuser.EditProfile(request);
            return Ok(result);
        }

       

    }
}
