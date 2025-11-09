using AutoMapper;
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
    public class GetAttendanceByIDQueryHandler : IRequestHandler<GetAttendanceByIDQuery, List<AttendanceLogDTO>>
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;
        public GetAttendanceByIDQueryHandler(IAttendanceRepository attendanceRepository, ICurrentUserService currentUser, IMapper mapper)
        {
            _attendanceRepository = attendanceRepository;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<List<AttendanceLogDTO>> Handle(GetAttendanceByIDQuery request, CancellationToken cancellationToken)
        {
          var userId = _currentUser.UserId; 
            if (userId == null)
                throw new UnauthorizedAccessException("User is not authenticated");
            var attendances = await _attendanceRepository.GetAttendanceByTokenAsync(userId);
            return _mapper.Map<List<AttendanceLogDTO>>(attendances);
        }
    }
}
