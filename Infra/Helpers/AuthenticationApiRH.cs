using Azure.Core;
using Data.Dtos;
using Infra.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Helpers
{
    public class AuthenticationApiRH
    {
        public static async Task<AuthenticaticationData> Login(LoginRequest loginRequest)
        {
            string url = string.Format("api/auth/login");
            AuthenticaticationData data = await ApiRequestBase<LoginRequest,AuthenticaticationData>.PostDiffRequest(url.route(Request.BookingSystem),loginRequest);
            return data;
        }
    }
}
