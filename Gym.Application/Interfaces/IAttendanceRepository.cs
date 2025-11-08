using Gym.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Application.Interfaces
{
    public interface IAttendanceRepository
    {
        Task AddAttendanceLogAsync(AttendanceLog log);
        Task<Memberships?> GetMembershipByIdAsync(int membershipId);
        Task<List<AttendanceLog>> GetCurrentAttendanceAsync();
        Task<List<AttendanceLog>> GetAllAttendanceAsync();
        Task<bool> HasScannedTodayAsync(int memberId, DateTime today);
        Task<List<AttendanceLog>> GetAttendanceByUserIdAsync(string userId);
    }
}
