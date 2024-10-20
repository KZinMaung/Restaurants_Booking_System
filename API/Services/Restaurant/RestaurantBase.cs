﻿using API.Services.Customer;
using Core.Extension;
using Data.Constants;
using Data.Dtos;
using Data.Model;
using Data.Models;
using Infra.BlobStorage;
using Infra.Helpers;
using Infra.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Restaurant
{
    public class RestaurantBase:IRestaurant
    {
        private readonly BookingSystemDbCotnext _context;
        private readonly IConfiguration _configuration;
        private IAzureBlobStorage _azureBlobStorage;
        UnitOfWork _uow;
       

        public RestaurantBase(BookingSystemDbCotnext context, IAzureBlobStorage azureBlobStorage, IConfiguration configuration)
        {
            this._context = context;
            this._uow = new UnitOfWork(_context);
            this._configuration = configuration;
            this._azureBlobStorage = azureBlobStorage;
            
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }


        public async Task<ResponseData> UpSert(tbRestaurant restaurant)
        {
            tbRestaurant entity;
            ResponseData response = new ResponseData();
            //update
            if (restaurant.Id != 0)
            {
               
                restaurant.Accesstime = MyExtension.GetLocalTime();
                entity = await _uow.restaurantRepo.UpdateAsync(restaurant);
            }
            //insert
            else
            {
                restaurant.Password = HashPassword(restaurant.Password);
                restaurant.Accesstime = MyExtension.GetLocalTime();
                restaurant.CreatedAt = MyExtension.GetLocalTime();
                entity = await _uow.restaurantRepo.InsertReturnAsync(restaurant);
            }


            response.Status = entity != null ? ResponseStatus.Success : ResponseStatus.Fail;
            return response;

        }


        public async Task<tbRestaurant> GetById(int id)
        {
            return await _uow.restaurantRepo.GetAll()
                .FirstOrDefaultAsync(a => a.Id == id && a.IsDeleted != true) ?? new tbRestaurant();
        }
    }
}
