using Attendance.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Core.Repositories
{
    public interface IRecordRepository
    {
        bool BulkInsertRecords(List<Record> records);
        List<Record> GetRecordsByUserId(User user);
        List<DateTime> AllDays();
        int GetRecordsCount();
    }
}
