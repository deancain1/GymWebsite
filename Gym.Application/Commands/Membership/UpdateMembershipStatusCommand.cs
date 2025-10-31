using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Application.Commands.Membership
{
    public class UpdateMembershipStatusCommand : IRequest
    {
        public int MemberID { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
