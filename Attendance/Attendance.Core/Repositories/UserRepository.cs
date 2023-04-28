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
    public class UserRepository:IUserRepository
    {
        private readonly MyDbContext context;
        public UserRepository(MyDbContext context)
        {
            this.context = context;
        }

        public bool BulkCreateUser(List<User> users)
        {
            try
            {
                context.BulkInsert(users);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CreateUser(User user)
        {
            try
            {
                context.Users.Add(user);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public int GetRecordsCount()
        {
            return context.Users.Count();
        }

        public List<User> GetUsers()
        {
            return context.Users.ToList();
        }
    }
}
