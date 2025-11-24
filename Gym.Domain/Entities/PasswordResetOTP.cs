using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Domain.Entities
{
    public  class PasswordResetOTP
    {
        public int Id { get; set; }
        public string UserId { get; set; } = default!;
        public string Email { get; set; }
        public string OTP { get; set; }
        public DateTime ExpirationTime { get; set; }
        public bool IsUsed { get; set; } = false;
    }
}
