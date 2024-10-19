using Data.Constants;
using Data.Dtos;
using Data.Models;
using Infra.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Controllers
{
    public class RestaurantController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var result = User.Identity.IsAuthenticated.ToString();
            int id = 0;

            if(result == "True")
            {
                id = int.Parse(User.Claims.ToArray()[AuthDataIndex.Id].Value);
            }
            tbRestaurant res = await RestaurantApiRH.GetById(id);
            return View(res);
        }

        public IActionResult LoadMenu()
        {
            return PartialView("_Menu");
        }

        public IActionResult LoadAbout()
        {
            return PartialView("_About");
        }

        public IActionResult LoadReviews()
        {
            return PartialView("_Reviews");
        }

        public IActionResult Register()
        {
            return View(new tbRestaurant());
        }

        public IActionResult Bookings()
        {
            return View();
        }

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


        public async Task<IActionResult> UpSert(tbRestaurant res)
        {
            if (res.CoverPhotoString != null)
            {
                res.CoverPhoto = UploadPhoto(res.CoverPhotoString);
            }

            if (res.ProfilePhotoString != null)
            {
                res.ProfilePhoto = UploadPhoto(res.ProfilePhotoString);
            }

            ResponseData result = await RestaurantApiRH.UpSert(res);
           
            return Json(result);
        }


        public async Task<IActionResult> GetById(int id)
        {
            tbRestaurant result = await RestaurantApiRH.GetById(id);
            return Ok(result);
        }
    }
}
