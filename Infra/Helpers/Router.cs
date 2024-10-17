using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Helpers
{
    public struct BaseUrl
    {
      
       public const string BookingSystem = "https://localhost:7171/";
     
    }
    public struct Request
    {
        public const string BookingSystem = "BookingSystem";
       
    }

    public static class Router
    {

        public static string route(this string Route, string project)
        {
            string route = null;
            switch (project)
            {
                case Request.BookingSystem:
                    route = $"{BaseUrl.BookingSystem}{Route}";
                    break;
               
                default:
                    throw new ArgumentException("Invalid Route");
            }
            return route;
        }
    }
}
