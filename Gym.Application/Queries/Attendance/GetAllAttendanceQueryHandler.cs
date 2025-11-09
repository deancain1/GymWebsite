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
    public class GetAllAttendanceQueryHandler : IRequestHandler<GetAllAttendanceQuery, List<AttendanceLogDTO>>
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IMapper _mapper;
        public GetAllAttendanceQueryHandler(IAttendanceRepository attendanceRepository, IMapper mapper)
        {
            _attendanceRepository = attendanceRepository;
            _mapper = mapper;
        }
        public async Task<List<AttendanceLogDTO>> Handle(GetAllAttendanceQuery request, CancellationToken cancellationToken)
        {
            var attendance = await _attendanceRepository.GetAllAttendanceAsync();
            return _mapper.Map<List<AttendanceLogDTO>>(attendance);
        }
    }
}
