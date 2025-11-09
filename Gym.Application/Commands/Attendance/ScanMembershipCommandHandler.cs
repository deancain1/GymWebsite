using AutoMapper;
using Gym.Application.Interfaces;
using Gym.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Application.Commands.Attendance
{
    public class ScanMembershipCommandHandler : IRequestHandler<ScanMembershipCommand, string>
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IMembershipsRepository _membershipsRepository;
        private readonly IMapper _mapper;
        public ScanMembershipCommandHandler(IAttendanceRepository attendanceRepository, IMembershipsRepository membershipsRepository, IMapper mapper)
        {
            _attendanceRepository = attendanceRepository;
            _membershipsRepository = membershipsRepository;
            _mapper = mapper;
        }

        public async Task<string> Handle(ScanMembershipCommand request, CancellationToken cancellationToken)
        {
            var membership = await _membershipsRepository.GetMemberByIDAsync(request.MemberID);
            if (membership == null)
                return "Membership not found";

            if (membership.ExpirationDate < DateTime.Now)
                return "Membership Expired";

            var today = DateTime.Now.Date;
            /*
            var alreadyScanned = await _attendanceRepository.HasScannedTodayAsync(membership.MemberID, today);
            if (alreadyScanned)
                return "Already scanned today";
            */
            var log = _mapper.Map<AttendanceLog>(membership);

            await _attendanceRepository.AddAttendanceLogAsync(log);

            return "Attendance recorded successfully";
        }
    }
}
