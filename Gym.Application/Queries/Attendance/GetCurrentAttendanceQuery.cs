using Gym.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Application.Queries.Attendance
{
    public class GetCurrentAttendanceQuery : IRequest<List<AttendanceLogDTO>>
    {
    }
}
