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
    public class GetAllAttendanceQueryHandler : IRequestHandler<GetAllAttendanceQuery, List<AttendanceLogDTO>>
    {
        private readonly IAttendanceRepository _attendanceRepository;
        public GetAllAttendanceQueryHandler(IAttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }
        public async Task<List<AttendanceLogDTO>> Handle(GetAllAttendanceQuery request, CancellationToken cancellationToken)
        {
            var attendance = await _attendanceRepository.GetAllAttendanceAsync();
            return attendance.Select(attendance => new AttendanceLogDTO
            {
                MemberID = attendance.MemberID,
                FullName = attendance.FullName,
                UserId = attendance.UserId,
                ScanTime = attendance.ScanTime,
            }).ToList();
        }
    }
}
