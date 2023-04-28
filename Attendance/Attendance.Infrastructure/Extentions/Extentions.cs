using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Infrastructure.Extentions
{
    public static class Extentions
    {
        public static string ConvertToPersian(this DateTime englishDate)
        {
            var p = new PersianCalendar();
            string month = (p.GetMonth((DateTime)englishDate)).ToString();
            if (month.Length == 1)
            {
                month = "0" + p.GetMonth((DateTime)englishDate);
            }
            string day = (p.GetDayOfMonth((DateTime)englishDate)).ToString();
            if (day.Length == 1)
            {
                day = "0" + p.GetDayOfMonth((DateTime)englishDate);
            }
            string persianDate = p.GetYear((DateTime)englishDate) + "/" + month + "/" + day;
            return persianDate;
        }
        public static string PersionDayOfWeek(this DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                    return "شنبه";
                case DayOfWeek.Sunday:
                    return "یک شنبه";
                case DayOfWeek.Monday:
                    return "دو شنبه";
                case DayOfWeek.Tuesday:
                    return "سه شنبه";
                case DayOfWeek.Wednesday:
                    return "چهار شنبه";
                case DayOfWeek.Thursday:
                    return "پنج شنبه";
                case DayOfWeek.Friday:
                    return "جمعه";
                default:
                    throw new Exception();
            }
        }
    }
}
