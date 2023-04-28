using Attendance.Core.Models;
using Attendance.Core.Repositories;
using Attendance.Infrastructure.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Infrastructure.Services
{
    public interface IRecordService:IRecordRepository
    {
        List<UserRecordDTO> GetUsersReport(List<User> users);
        Tuple<string, string> GetUserTypeAndUseFulTimeByRecord(List<Record> records);
    }
}
