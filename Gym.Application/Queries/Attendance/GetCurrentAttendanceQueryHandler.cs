using Gym.Application.DTOs;
using Gym.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Application.Queries.Attendance
{
    public class GetCurrentAttendanceQueryHandler : IRequestHandler<GetCurrentAttendanceQuery, List<AttendanceLogDTO>>
    {
        private readonly IAttendanceRepository _attendanceRepository;
        public GetCurrentAttendanceQueryHandler(IAttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }
        public async Task<List<AttendanceLogDTO>> Handle(GetCurrentAttendanceQuery request, CancellationToken cancellationToken)
        {
            var attendance = await _attendanceRepository.GetCurrentAttendanceAsync();
            return attendance.Select(attendance => new AttendanceLogDTO
            {
                AttendanceID = attendance.AttendanceID,
                FullName = attendance.FullName,
                MemberID = attendance.MemberID,
                UserId
                = attendance.UserId,
                ScanTime = attendance.ScanTime
            }).ToList();
        }
    }
}
