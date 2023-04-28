using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Infrastructure.Dto
{
    public class UserRecordDTO
    {
        public DateTime date { get; set; }
        public string persianDate { get; set; }
        public string dayOfWeek { get; set; }
        public string userName { get; set; }
        public string type { get; set; }
        public string usefulTime { get; set; }
        public string entryTime { get; set; }
        public string exitTime { get; set; }
        public List<string> recordsTimes { get; set; }
    }
}
