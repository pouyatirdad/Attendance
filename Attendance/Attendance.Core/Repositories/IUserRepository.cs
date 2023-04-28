using Attendance.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Core.Repositories
{
    public interface IUserRepository
    {
        bool CreateUser(User user);
        bool BulkCreateUser(List<User> users);
        List<User> GetUsers();
        int GetRecordsCount();
    }
}
