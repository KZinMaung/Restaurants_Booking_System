using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extension
{
    public static class MyExtension
    {
      
        public static string GetUniqueNumber()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            string uniquecode = BitConverter.ToUInt32(buffer, 12).ToString();
            return uniquecode;
        }

        public static DateTime GetLocalTime()
        {
            DateTime utc = DateTime.UtcNow;
            return TimeZoneInfo.ConvertTimeFromUtc(utc, TimeZoneInfo.FindSystemTimeZoneById("Myanmar Standard Time"));
        }
        public static string GetUniqueCode()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToUInt32(buffer, 12).ToString();
        }

        public static DateTime GetMMTime()
        {
            DateTime utc = DateTime.UtcNow;
            return TimeZoneInfo.ConvertTimeFromUtc(utc, TimeZoneInfo.FindSystemTimeZoneById("Myanmar Standard Time"));

        }


        public static string GetDateAgo(DateTime dateTime)
        {
            TimeSpan timeSpan = DateTime.Now.Subtract(dateTime);

            if (timeSpan.TotalSeconds < 0)
            {
                return "1 s";
            }

            return timeSpan.TotalSeconds switch
            {
                <= 60 => $"{timeSpan.Seconds} s",

                _ => timeSpan.TotalMinutes switch
                {
                    <= 1 => "1 m",
                    < 60 => $"{timeSpan.Minutes} m",
                    _ => timeSpan.TotalHours switch
                    {
                        <= 1 => "1 h",
                        < 24 => $"{timeSpan.Hours} h",
                        _ => timeSpan.TotalDays switch
                        {
                            <= 1 => "1 d",
                            <= 30 => $"{timeSpan.Days} d",

                            <= 60 => "1 m",
                            < 365 => $"{timeSpan.Days / 30} mon",

                            <= 365 * 2 => "1 y",
                            _ => $"{timeSpan.Days / 365} yrs"
                        }
                    }
                }
            };
        }

        public static string GetHourMinFormat(int totalMin)
        {
            int hours = 0;
            int mins = 0;
            if(totalMin < 60)
            {
                return $"{totalMin} mins";
            }
            else if(totalMin == 60)
            {
                return "1 hr";
            }
            {
                hours = (totalMin) / 60;
                mins = (totalMin) % 60;

                return $"{hours} {(hours > 1 ? "hrs": "hr")} {mins} {(mins > 1 ? "mins": "min")}";
            }
        }


        public static string AddCommaToStringForSearching(this string cmstring)
        {
            return $@",{cmstring},";
        }

        public static string AddCommaToCommaStringToSave(this string cmstring)
        {
            return $@",{cmstring},";
        }
        public static string RemoveCommaFromCommaStringForDisplay(this string cmstring)
        {
            return cmstring.Substring(1, cmstring.Length - 2);
        }

       
    }
}
