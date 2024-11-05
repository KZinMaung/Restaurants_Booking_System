using API.Services.Menu;
using API.Services.Restaurant;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    [ApiController]
    public class MenuController : ControllerBase
    {
        IMenu _imenu;
        public MenuController(IMenu imenu)
        {
            this._imenu = imenu;
        }

        [HttpPost("api/menu/upsert")]
        public async Task<IActionResult> UpSert(tbMenu menu)
        {
            var result = await this._imenu.UpSert(menu);
            return Ok(result);
        }

        [HttpGet("api/menu/get-list")]
        public async Task<IActionResult> GetList(int resId = 0, int page = 1, int pageSize = 10, string? sortVal = "Id", string? sortDir = "asc",
                                string? q = "")

        {
            var result = await this._imenu.GetList(resId, page, pageSize, sortVal, sortDir, q);
            return Ok(result);
        }


        [HttpGet("api/menu/get-by-id")]
        public async Task<IActionResult> GetById(int id)

        {
            var result = await this._imenu.GetById(id);
            return Ok(result);
        }



        [HttpGet("api/menu/delete")]
        public async Task<IActionResult> Delete(int id = 0)
        {
            var result = await this._imenu.Delete(id);
            return Ok(result);
        }


    }
}
