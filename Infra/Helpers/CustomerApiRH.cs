using Data.Dtos;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Helpers
{
    public class CustomerApiRH
    {
        public static async Task<tbCustomer> CreateCustomer(tbCustomer customer)
        {
     
            string url = string.Format("api/customer/create-customer");
            tbCustomer data = await ApiRequestBase<tbCustomer>.PostRequest(url.route(Request.BookingSystem), customer);
            return data;
        }

       
    }
}
