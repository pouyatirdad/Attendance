using Attendance.Core.Context;
using Attendance.Core.Models;
using EFCore.BulkExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Core.Repositories
{
    public class RecordRepository:IRecordRepository
    {
        private readonly MyDbContext context;
        public RecordRepository(MyDbContext context)
        {
            this.context = context;
        }
        public bool BulkInsertRecords(List<Record> records)
        {
            try
            {
                context.BulkInsert(records);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Record> GetRecordsByUserId(User user)
        {
            return context.Record.Where(x => x.UserId == user.Id).OrderBy(x => x.Date.Date).ToList();
        }

        public List<DateTime> AllDays()
        {
            return context.Record.Select(x=>x.Date).ToList();
        }

        public int GetRecordsCount()
        {
            return context.Record.Count();
        }
    }
}
