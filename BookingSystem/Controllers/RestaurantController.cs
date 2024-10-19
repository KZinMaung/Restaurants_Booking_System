using Data.Constants;
using Data.Dtos;
using Data.Models;
using Infra.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Controllers
{
    public class RestaurantController : Controller
    {
        public IActionResult Index()
        {
            return View();
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

        public async Task<IActionResult> UpSert(tbRestaurant res)
        {
            if (res.CoverPhotoString != null)
            {
                var ext = GetFileExtension(res.CoverPhotoString);

                string imageName = Guid.NewGuid() + "." + ext;
                res.CoverPhoto = imageName;

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

                byte[] imageBytes = Convert.FromBase64String(res.CoverPhotoString);

                System.IO.File.WriteAllBytes(imgPath, imageBytes);


            }

            ResponseData result = await RestaurantApiRH.UpSert(res);
           
            return Json(result);
        }
    }
}
