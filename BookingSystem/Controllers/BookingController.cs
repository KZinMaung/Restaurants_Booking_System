﻿using Data.Constants;
using Data.Dtos;
using Data.Models;
using Data.ViewModels;
using Infra.Helpers;
using Infra.Services;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookingSystem.Controllers
{
    public class BookingController : Controller
    {
        public async Task<IActionResult> GetAvailableCount(int resId, int resScheId, DateTime bookingDate)
        {
            int count = await BookingApiRH.GetAvailableCount(resId, resScheId, bookingDate);
            return Ok(count);
        }

        public async Task<IActionResult> UpSert(tbBooking data)
        {
           
            ResponseData result = await BookingApiRH.UpSert(data);
            return Json(result);
        }

        public async Task<IActionResult> GetList(string userType , int page = 1, int pageSize = 10, string? q = "")
        {
            var result = User.Identity.IsAuthenticated.ToString();
            int id = 0;
            int resId = 0;
            int cusId = 0;

            if (result == "True")
            {
                id = int.Parse(User.Claims.ToArray()[AuthDataIndex.Id].Value);
            }
            
            if(userType == UserType.Restaurant) {
                resId = id;
                PagedListClient<BookingVM> bookings = await BookingApiRH.GetList(cusId, resId, page, pageSize, q);
                return PartialView("_RestaurantBookings", bookings);
            }
            else
            {
                cusId = id;
                PagedListClient<BookingVM> bookings = await BookingApiRH.GetList(cusId, resId, page, pageSize, q);
                return PartialView("_CustomerBookings", bookings);
            }

            
        }

        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            ResponseData result = await BookingApiRH.CancelBooking(bookingId);
            return Json(result);
        }


        public async Task<IActionResult> CompleteBooking(int bookingId)
        {
            ResponseData result = await BookingApiRH.CompleteBooking(bookingId);
            return Json(result);
        }

    }
}
