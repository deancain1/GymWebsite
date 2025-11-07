using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Application.DTOs
{
    public class MembershipDTO
    {
        public int MemberID { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Plan { get; set; } = string.Empty;
        public DateTime AppliedDate { get; set; } = DateTime.Now;
        public DateTime? StartDate { get; set; }
        public int DurationMonths { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string QRCode { get; set; } = string.Empty;
    }
}
