using Attendance.Core.Models;
using Attendance.Infrastructure.Dto;
using Attendance.Infrastructure.Extentions;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Infrastructure.Helper
{
    public static class Helper
    {
        public static async Task<string> UploadFile(IFormFile File)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", Guid.NewGuid() + Path.GetExtension(File.FileName));

            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await File.CopyToAsync(fileStream);
            }
            return filePath;
        }
        public static Tuple<List<User>, List<Record>> SeperateData(string data)
        {
            string text = System.IO.File.ReadAllText(data);

            string[] stringSeparators = new string[] { "\r\n" };
            string[] userData = text.Split(stringSeparators, StringSplitOptions.None);

            var users = new List<User>();
            var records = new List<Record>();

            try
            {
                foreach (var item in userData)
                {
                    string[] values = item.Split(new string[] { "\t" }, StringSplitOptions.None);

                    if (!values[0].Any() || !values[1].Any() || !values[2].Any())
                    {
                        continue;
                    }

                    Record record = new Record()
                    {
                        Date = DateTime.Parse(values[2]),
                        UserId = int.Parse(values[0]),
                    };

                    records.Add(record);

                    if (users.Any(x => x.Id == int.Parse(values[0])))
                    {
                        continue;
                    }
                    else
                    {
                        User user = new User()
                        {
                            Id = int.Parse(values[0]),
                            Name = values[1],
                        };
                        users.Add(user);
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return Tuple.Create(users, records);
        }
        public static byte[] DownloadExcelFile(List<string> headerData, List<UserRecordDTO> bodyData)
        {
            using (var workbook = new XLWorkbook())
            {
                workbook.RightToLeft = true;
                workbook.ColumnWidth = 200;
                var worksheet = workbook.Worksheets.Add("Report");
                int currentRow = 1;
                int headerColumn = 1;

                foreach (var header in headerData)
                {
                    worksheet.Cell(currentRow, headerColumn).Value = header;
                    headerColumn++;
                }
                foreach (var body in bodyData.OrderBy(x => x.date))
                {
                    var recordString = String.Join(" - ", body.recordsTimes);
                    worksheet.Cell(currentRow, 1).Value = currentRow;
                    worksheet.Cell(currentRow, 2).Value = body.persianDate;
                    worksheet.Cell(currentRow, 3).Value = body.dayOfWeek;
                    worksheet.Cell(currentRow, 4).Value = body.userName;
                    worksheet.Cell(currentRow, 5).Value = body.type;
                    worksheet.Cell(currentRow, 6).Value = body.usefulTime;
                    worksheet.Cell(currentRow, 7).Value = body.entryTime;
                    worksheet.Cell(currentRow, 8).Value = body.exitTime;
                    worksheet.Cell(currentRow, 9).Value = recordString;
                    currentRow++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return content;
                }
            }
        }
        public static bool TimeBetween(TimeSpan time, TimeSpan start, TimeSpan end)
        {
            if (time < start)
                return true;

            if (start < end)
                return start <= time && time <= end;

            return !(end < time && time < start);
        }
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
               (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}