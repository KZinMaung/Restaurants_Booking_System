using Data.Models;
using Infra.Helpers;
using Infra.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Controllers
{
    public class RestaurantScheduleController : Controller
    {
        public async Task<IActionResult> GetList(DateTime selectedDate, int resId = 0)
        {
            // Get current date and time
            DateTime currentDate = DateTime.Now.Date;
            TimeSpan currentTime = DateTime.Now.TimeOfDay;

            // Fetch all schedules
            List<tbRestaurantSchedule> schedules = await RestaurantScheduleApiRH.GetList(resId);

            // Apply filtering
            if (selectedDate.Date == currentDate)
            {
                schedules = schedules
                    .Where(schedule => schedule.StartTime > currentTime)
                    .ToList();
            }
       
            return PartialView("_ScheduleList", schedules);
        }

    }
}
