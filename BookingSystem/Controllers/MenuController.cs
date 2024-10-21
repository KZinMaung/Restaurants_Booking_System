using Data.Constants;
using Data.Dtos;
using Data.Models;
using Infra.Helpers;
using Infra.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Controllers
{
    public class MenuController : Controller
    {
        public static string GetFileExtension(string base64String)
        {
            var data = base64String.Substring(0, 5);

            switch (data.ToUpper())
            {
                case "IVBOR":
                    return "png";
                case "/9J/4":
                    return "jpg";
                case "AAAAF":
                    return "mp4";
                case "JVBER":
                    return "pdf";
                case "AAABA":
                    return "ico";
                case "UMFYI":
                    return "rar";
                case "E1XYD":
                    return "rtf";
                case "U1PKC":
                    return "txt";
                case "MQOWM":
                case "77U/M":
                    return "srt";
                default:
                    return string.Empty;
            }
        }
        public static string UploadPhoto(string photoString)
        {
            var ext = GetFileExtension(photoString);
            string imageName = Guid.NewGuid() + "." + ext;


            //set the image path
            string path =
          Path.GetFullPath(Path.Combine(Environment.CurrentDirectory,
          $"wwwroot\\Storage"));

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string imgPath = Path.Combine(Directory.GetCurrentDirectory(),
          $"wwwroot\\Storage", imageName);

            byte[] imageBytes = Convert.FromBase64String(photoString);
            System.IO.File.WriteAllBytes(imgPath, imageBytes);

            return imageName;

        }


        public async Task<IActionResult> UpSert(tbMenu menu)
        {
            var isAuthenticated = User.Identity.IsAuthenticated.ToString();
            int resId = 0;

            if (isAuthenticated == "True")
            {
                resId = int.Parse(User.Claims.ToArray()[AuthDataIndex.Id].Value);
            }
           
            if (menu.PhotoString != null)
            {
                menu.Photo = UploadPhoto(menu.PhotoString);
            }
           
            menu.RestaurantId = resId;
            ResponseData result = await MenuApiRH.UpSert(menu);
            return Json(result);
        }


        public async Task<IActionResult> GetList(int resId = 0, int page = 1, int pageSize = 10, string? q = "")
        {
            
            ViewBag.page = page;
            ViewBag.pagesize = pageSize;
            PagedListClient<tbMenu> result = await MenuApiRH.GetList(resId, page, pageSize, q);

            return PartialView("_MenuList", result);
        }

        public async Task<IActionResult> MenuForm(string FormType, int Id)
        {

            tbMenu menu = new tbMenu();
            if (FormType == "Add")
            {
                return PartialView("_MenuForm", menu);
            }

            else
            {
                tbMenu result = await MenuApiRH.GetById(Id);
                return PartialView("_MenuForm", result);
 
            }
        }


        public async Task<IActionResult> Delete(int id)
        {
            ResponseData result = await MenuApiRH.Delete(id);
            return Json(result);

        }

    }
}
