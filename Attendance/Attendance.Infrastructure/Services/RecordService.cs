using Attendance.Core.Context;
using Attendance.Core.Models;
using Attendance.Core.Repositories;
using Attendance.Infrastructure.Dto;
using Attendance.Infrastructure.Extentions;
using Attendance.Infrastructure.Helper;
using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Infrastructure.Services
{
    public class RecordService : IRecordService
    {
        private readonly IRecordRepository repository;
        public RecordService(IRecordRepository repository)
        {
            this.repository = repository;
        }
        public bool BulkInsertRecords(List<Record> records)
        {
            return repository.BulkInsertRecords(records);
        }

        public List<DateTime> AllDays()
        {
            return repository.AllDays();
        }

        public List<Record> GetRecordsByUserId(User user)
        {
            return repository.GetRecordsByUserId(user);
        }

        public List<UserRecordDTO> GetUsersReport(List<User> users)
        {
            var report = new List<UserRecordDTO>();
            foreach (var user in users)
            {
                List<Record> findedRecord;
                findedRecord = repository.GetRecordsByUserId(user);
                var usersRecordsByDay = new List<UserRecordDTO>();
                var activityDays = repository.AllDays().DistinctBy(x => x.Date).ToList();

                foreach (var activityDay in activityDays)
                {
                    UserRecordDTO userRecord = new UserRecordDTO();

                    var records = findedRecord.Where(x => x.Date.Day == activityDay.Day
                        && x.Date.Month == activityDay.Month
                        && x.Date.Year == activityDay.Year).ToList();

                    userRecord.date = activityDay.Date;
                    userRecord.persianDate = activityDay.Date.ConvertToPersian();
                    userRecord.dayOfWeek = activityDay.Date.PersionDayOfWeek().ToString();
                    userRecord.userName = user.Name;

                    var recordsTimes = new List<string>();

                    if (records.Any())
                    {
                        userRecord.type = GetUserTypeAndUseFulTimeByRecord(records).Item1;
                        userRecord.usefulTime = GetUserTypeAndUseFulTimeByRecord(records).Item2;
                        userRecord.entryTime = records.First().Date.ToString("HH:mm");
                        userRecord.exitTime = records.Last().Date.ToString("HH:mm");

                        foreach (var times in records)
                        {
                            string time = times.Date.ToString("HH:mm");
                            recordsTimes.Add(time);
                        }
                    }
                    else
                    {
                        userRecord.type = "مرخصی روزانه";
                    }

                    userRecord.recordsTimes = recordsTimes;

                    usersRecordsByDay.Add(userRecord);
                }
                report.AddRange(usersRecordsByDay);
            }
            return report;
        }

        public Tuple<string, string> GetUserTypeAndUseFulTimeByRecord(List<Record> records)
        {
            if (records.Count() % 2 != 0)
            {
                return Tuple.Create("خطا", "خطا");
            }
            else
            {
                TimeSpan startTime = new TimeSpan(8, 30, 0);
                TimeSpan endTime = new TimeSpan(8, 45, 0);
                TimeSpan userEntryTime = records.First().Date.TimeOfDay;
                TimeSpan userExitTime = records.First().Date.TimeOfDay;
                TimeSpan dayWorkHour = new TimeSpan(8, 30, 0);

                TimeSpan useFulTime = records.Last().Date - records.First().Date;

                if (!Helper.Helper.TimeBetween(userEntryTime, startTime, endTime))
                {
                    return Tuple.Create("تاخیر", useFulTime.ToString());
                }
                if (records.Count() > 2)
                {
                    double leftTime = 0;
                    for (int i = 0; i < records.Count(); i++)
                    {
                        i++;
                        if (i == records.Count() - 1)
                        {
                            break;
                        }
                        leftTime = records[i + 1].Date.Subtract(records[i].Date).TotalMinutes;
                    }
                    useFulTime = TimeSpan.FromMinutes(useFulTime.TotalMinutes - leftTime);
                    return Tuple.Create("مرخصی ساعتی", useFulTime.ToString());
                }
                if (useFulTime < dayWorkHour)
                {
                    return Tuple.Create("تاخیر", useFulTime.ToString());
                }
                return Tuple.Create("عادی", useFulTime.ToString());
            }
        }

        public int GetRecordsCount()
        {
            return repository.GetRecordsCount();
        }
    }
}
