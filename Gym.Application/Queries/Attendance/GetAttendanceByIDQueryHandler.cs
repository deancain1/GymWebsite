using Gym.Application.DTOs;
using Gym.Application.Interfaces;
using Gym.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Application.Queries.Attendance
{
    public class GetAttendanceByIDQueryHandler : IRequestHandler<GetAttendanceByIDQuery, List<AttendanceLogDTO>>
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly ICurrentUserService _currentUser;
        public GetAttendanceByIDQueryHandler(IAttendanceRepository attendanceRepository, ICurrentUserService currentUser)
        {
            _attendanceRepository = attendanceRepository;
            _currentUser = currentUser;
        }

        public async Task<List<AttendanceLogDTO>> Handle(GetAttendanceByIDQuery request, CancellationToken cancellationToken)
        {
          var userId = _currentUser.UserId; 
            if (userId == null)
                throw new UnauthorizedAccessException("User is not authenticated");
            var attendances = await _attendanceRepository.GetAttendanceByUserIdAsync(userId);

           
            return attendances.Select(a => new AttendanceLogDTO
            {
                UserId = a.UserId,
                MemberID = a.MemberID,
                FullName = a.FullName,
                ScanTime = a.ScanTime 
            }).ToList();
        }
    }
}
