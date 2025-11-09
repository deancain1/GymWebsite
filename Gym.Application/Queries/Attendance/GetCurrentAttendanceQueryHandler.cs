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
    public class GetCurrentAttendanceQueryHandler : IRequestHandler<GetCurrentAttendanceQuery, List<AttendanceLogDTO>>
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IMapper _mapper;
        public GetCurrentAttendanceQueryHandler(IAttendanceRepository attendanceRepository, IMapper mapper)
        {
            _attendanceRepository = attendanceRepository;
            _mapper = mapper;
        }
        public async Task<List<AttendanceLogDTO>> Handle(GetCurrentAttendanceQuery request, CancellationToken cancellationToken)
        {
            var attendance = await _attendanceRepository.GetCurrentAttendanceAsync();
            return _mapper.Map<List<AttendanceLogDTO>>(attendance);
        }
    }
}
