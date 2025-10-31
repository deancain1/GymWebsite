using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Domain.Entities
{
    public class AttendanceLog
    {
        [Key]
        public int AttendanceID { get; set; }
        public int MemberID { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public DateTime ScanTime { get; set; }
        public string Status { get; set; }
    }
}
