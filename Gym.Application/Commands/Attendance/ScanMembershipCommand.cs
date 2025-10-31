using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Application.Commands.Attendance
{
    public class ScanMembershipCommand : IRequest<string>
    {
        public int MemberID { get; set; }
    }
}
