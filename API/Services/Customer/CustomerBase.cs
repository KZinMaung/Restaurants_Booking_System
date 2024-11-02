
using Core.Extension;
using Data.Constants;
using Data.Dtos;
using Data.Model;
using Data.Models;
using Infra.BlobStorage;
using Infra.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Customer
{
    public class CustomerBase : ICustomer
    {
        private readonly BookingSystemDbCotnext _context;
        private readonly IConfiguration _configuration;
        private IAzureBlobStorage _azureBlobStorage;
        UnitOfWork _uow;

        public CustomerBase(BookingSystemDbCotnext context, IAzureBlobStorage azureBlobStorage, IConfiguration configuration)
        {
            this._context = context;
            this._uow = new UnitOfWork(_context);
            this._configuration = configuration;
            this._azureBlobStorage = azureBlobStorage;
        }

        private bool VerifyPassword(string inputPassword, string storedHash)
        {

            return BCrypt.Net.BCrypt.Verify(inputPassword, storedHash);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }


        public async Task<tbCustomer> CreateCustomer(tbCustomer customer)
        {

            tbCustomer customerByEmail = await _uow.customerRepo.GetAll().Where(a => a.Email == customer.Email && a.IsDeleted != true).FirstOrDefaultAsync() ?? new tbCustomer();
            tbCustomer customerByName = await _uow.customerRepo.GetAll().Where(a => a.Name == customer.Name && a.IsDeleted != true).FirstOrDefaultAsync() ?? new tbCustomer();
            if (customerByEmail.Id != 0 || customerByName.Id != 0)
            {
                throw new ArgumentException($"customer with email {customer.Email} or customername {customer.Name} already exists.");

            }
            else
            {
                tbCustomer entity = new tbCustomer
                {
                    Name = customer.Name,
                    Email = customer.Email,
                    Phone = customer.Phone,
                    Password = HashPassword(customer.Password),
                    CreatedAt = DateTime.Now,
                    Accesstime = DateTime.Now,
                    IsDeleted = false
                };
                var result = await _uow.customerRepo.InsertReturnAsync(entity);
                return result;
            }

        }



        public async Task<tbCustomer> GetCustomerById(int id)
        {
            return await _uow.customerRepo.GetAll()
                .FirstOrDefaultAsync(a => a.Id == id && a.IsDeleted != true) ?? new tbCustomer();
        }

        public async Task<ResponseData> UpSert(tbCustomer customer)
        {
            tbCustomer entity;
            ResponseData response = new ResponseData();
            //update
            if (customer.Id != 0)
            {

                customer.Accesstime = MyExtension.GetLocalTime();
                entity = await _uow.customerRepo.UpdateAsync(customer);
            }
            //insert
            else
            {

               entity =  await CreateCustomer(customer);
            }


            response.Status = entity != null ? ResponseStatus.Success : ResponseStatus.Fail;
            return response;

        }

    }
}
