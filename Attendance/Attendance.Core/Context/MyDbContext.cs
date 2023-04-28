using Attendance.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Core.Context
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Record> Record { get; set; }
    }
}
