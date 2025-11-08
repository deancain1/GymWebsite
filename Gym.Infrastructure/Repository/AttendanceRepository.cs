using Gym.Application.Interfaces;
using Gym.Domain.Entities;
using Gym.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Infrastructure.Repository
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly AppDbContext _context;
        public AttendanceRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task AddAttendanceLogAsync(AttendanceLog log)
        {
            _context.Attendance.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AttendanceLog>> GetAllAttendanceAsync()
        {
            return await _context.Attendance.ToListAsync();
        }

        public async Task<List<AttendanceLog>> GetCurrentAttendanceAsync()
        {
            var today = DateTime.Now.Date;
            return await _context.Attendance
            .Where(a => a.ScanTime >= today)
            .ToListAsync();
        }

        public async Task<Memberships?> GetMembershipByIdAsync(int membershipId)
        {
            return await _context.Memberships
            .FirstOrDefaultAsync(m => m.MemberID == membershipId);
        }
        public async Task<bool> HasScannedTodayAsync(int memberId, DateTime today)
        {
            return await _context.Attendance
                .AnyAsync(a => a.MemberID == memberId && a.ScanTime.Date == today);
        }

        public async Task<List<AttendanceLog>> GetAttendanceByUserIdAsync(string userId)
        {
           return await _context.Attendance
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.ScanTime)
                .ToListAsync();
        }
    }
}
