using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Domain.Entities
{
    public class Memberships
    {
        [Key]
        public int MemberID { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Plan { get; set; }
        public string Status { get; set; } = "New";
        public DateTime AppliedDate { get; set; } = DateTime.Now;
        public DateTime? StartDate { get; set; }
        public int DurationMonths { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string? QRCode { get; set; }
    }
}
