using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
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
        public ScanMembershipCommandHandler(IAttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }

        public async Task<string> Handle(ScanMembershipCommand request, CancellationToken cancellationToken)
        {
            var membership = await _attendanceRepository.GetMembershipByIdAsync(request.MemberID);
            if (membership == null)
                return "Membership not found";

            if (membership.ExpirationDate < DateTime.UtcNow)
                return "Membership Expired";

            var today = DateTime.UtcNow.Date;
            /*
            var alreadyScanned = await _attendanceRepository.HasScannedTodayAsync(membership.MemberID, today);
            if (alreadyScanned)
                return "Already scanned today";
            */
            var log = new AttendanceLog
            {
                MemberID = membership.MemberID,
                UserId = membership.UserId,
                FullName = membership.FullName,
                ScanTime = DateTime.UtcNow,
                Status = "Valid"
            };

            await _attendanceRepository.AddAttendanceLogAsync(log);

            return "Attendance recorded successfully";
        }
    }
}
