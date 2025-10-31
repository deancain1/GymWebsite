using Gym.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Application.Commands.Membership
{
    public class CreateMembershipCommand : IRequest<MembershipDTO>
    {
        public string UserId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Plan { get; set; } = string.Empty;
        public DateTime AppliedDate { get; set; } = DateTime.Now;
        public int DurationMonths { get; set; }
    }
}
