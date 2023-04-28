using Attendance.Core.Context;
using Attendance.Core.Models;
using System;
using System.Collections.Generic;
using EFCore.BulkExtensions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Core.Repositories;

namespace Attendance.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository repository;
        public UserService(IUserRepository repository)
        {
            this.repository = repository;
        }

        public bool BulkCreateUser(List<User> users)
        {
            return repository.BulkCreateUser(users);
        }

        public bool CreateUser(User user)
        {
            return repository.CreateUser(user);
        }

        public int GetRecordsCount()
        {
            return repository.GetRecordsCount();
        }

        public List<User> GetUsers()
        {
            return repository.GetUsers();
        }
    }
}
