using API.Services.Customer;
using Data.Dtos;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    [ApiController]
    public class CustomerController : ControllerBase
    {

        ICustomer _icustomer;
        public CustomerController(ICustomer icustomer)
        {
            this._icustomer = icustomer;
        }

        [HttpPost("api/customer/create-customer")]
        public async Task<IActionResult> CreateCustomer(tbCustomer customer)
        {
            var result = await this._icustomer.CreateCustomer(customer);
            return Ok(result);
        }

        [HttpGet("api/customer/get-by-id")]
        public async Task<IActionResult> GetCustomerById(int id)

        {
            var result = await this._icustomer.GetCustomerById(id);
            return Ok(result);
        }

        [HttpPut("api/users/{id}")]
        public async Task<IActionResult> UpdateCustomer([FromBody] UpdateCustomerRequest request)
        {
            var result = await this._icustomer.UpdateCustomer(request);
            return Ok(result);
        }
    }
}
