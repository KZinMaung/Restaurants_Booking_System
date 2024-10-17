
using Data.Dtos;
using Data.Models;

namespace API.Services.Customer
{
    public interface ICustomer
    {
        Task<tbCustomer> CreateCustomer(tbCustomer customer);

        Task<tbCustomer> GetCustomerById(int id);
        Task<tbCustomer> UpdateCustomer(UpdateCustomerRequest request);
    }
} 
