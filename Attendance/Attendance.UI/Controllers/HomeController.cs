using Attendance.Core.Models;
using Attendance.Infrastructure.Dto;
using Attendance.Infrastructure.Extentions;
using Attendance.Infrastructure.Helper;
using Attendance.Infrastructure.Services;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using static System.Net.WebRequestMethods;

namespace Attendance.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRecordService _recordService;
        public HomeController(IUserService userService, IRecordService recordService)
        {
            this._userService = userService;
            this._recordService = recordService;
        }

        public IActionResult Index()
        {
            ViewBag.UserCount = _userService.GetRecordsCount();
            ViewBag.RecordCount = _recordService.GetRecordsCount();
            return View();
        }
        [HttpPost]
        public async Task<string> UploadRecord(IFormFile File)
        {
            if (!File.ContentType.Equals("text/plain"))
            {
                return "فایل ارسالی مشکل دارد";
            }
            string filePath = await Helper.UploadFile(File);

            var data = Helper.SeperateData(filePath);

            if (!data.Item1.Any() || !data.Item2.Any())
            {
                return "فایل ارسالی خالی است";
            }

            bool result1 = _userService.BulkCreateUser(data.Item1);
            bool result2 = _recordService.BulkInsertRecords(data.Item2);

            if (!result1 && !result2)
                return "درج دیتا انجام نشد";

            return "موفقیت آمیز";
        }
        public IActionResult DownloadReport()
        {
            var users = _userService.GetUsers();
            var report = _recordService.GetUsersReport(users);
            List<string> headers = new List<string> { "ردیف", "تاریخ", "روز", "نام", "نوع", "کارکرد", "اولین ورود", "آخرین خروج", "رکورد ها" }; 
            var excelFile = Helper.DownloadExcelFile(headers,report);
            return File(
                        excelFile,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Report.xlsx");
        }
    }
}
