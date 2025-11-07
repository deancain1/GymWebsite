using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Application.DTOs
{
    public class AttendanceLogDTO
    {
        public int AttendanceID { get; set; }
        public int MemberID { get; set; }
        public string FullName { get; set; }
        public string UserId { get; set; }
        public DateTime ScanTime { get; set; }
        public string Status { get; set; }
       
    }
}
