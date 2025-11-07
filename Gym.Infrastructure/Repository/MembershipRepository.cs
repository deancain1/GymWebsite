using Gym.Domain.Entities;
using Gym.Domain.Interfaces;
using Gym.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Infrastructure.Repository
{
    public class MembershipRepository : IMembershipRepository
    {
        private readonly AppDbContext _context;
        public MembershipRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateMembershipAsync(Memberships memberships)
        {
            _context.Memberships.Add(memberships);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Memberships>> GetAllMembershipsAsync()
        {
            return await _context.Memberships.ToListAsync();
        }

        public async Task<Memberships?> GetMemberByIDAsync(int memberID)
        {
            return await _context.Memberships.FindAsync(memberID);
        }

        public async Task UpdateStatusAsync(Memberships member)
        {
            _context.Memberships.Update(member);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Memberships>> GetMembershipsByUserIdAsync(string userId)
        {
            return await _context.Memberships
                .Where(m => m.UserId == userId)
                .ToListAsync();
        }
        public async Task<List<Memberships>> GetActiveMembershipsAsync()
        {
            return await _context.Memberships
                .Where(m => m.Status != "Expired")
                .ToListAsync();
        }

        public async Task<int> GetTotalMembershipsAsync()
        {
            return await _context.Memberships
                .Where(m => m.Status == "Accepted")
                .CountAsync();
        }

        public async Task<Dictionary<string, int>> GetMembershipsPerMonthAsync()
        {
            var memberships = await _context.Memberships
                .Where(m => m.StartDate != null && m.Status == "Accepted") 
                .GroupBy(m => new { Year = m.StartDate.Value.Year, Month = m.StartDate.Value.Month })
                .Select(g => new
                {
                    Month = new DateTime(g.Key.Year, g.Key.Month, 1, 0, 0, 0, DateTimeKind.Utc).ToString("MMM yyyy"),
                    Count = g.Count()
                })
                .ToListAsync();

            return memberships.ToDictionary(x => x.Month, x => x.Count);
        }
        public async Task<Dictionary<string, int>> GetExpiredMembershipsPerMonthAsync()
        {
            var memberships = await _context.Memberships
                .Where(m => m.StartDate != null && m.Status == "Expired") 
                .GroupBy(m => new { Year = m.StartDate.Value.Year, Month = m.StartDate.Value.Month })
                .Select(g => new
                {
                    Month = new DateTime(g.Key.Year, g.Key.Month, 1, 0, 0, 0, DateTimeKind.Utc).ToString("MMM yyyy"),
                    Count = g.Count()
                })
                .ToListAsync();

            return memberships.ToDictionary(x => x.Month, x => x.Count);
        }

        public async Task<Dictionary<string, int>> GetMembershipPlanCountsAsync()
        {
            var counts = new Dictionary<string, int>();

            
            counts["Standard"] = await _context.Memberships.CountAsync(m => m.Plan == "Standard");
            counts["Premium"] = await _context.Memberships.CountAsync(m => m.Plan == "Premium");
            counts["VIP"] = await _context.Memberships.CountAsync(m => m.Plan == "VIP");
            counts["Student"] = await _context.Memberships.CountAsync(m => m.Plan == "Student");

            return counts;
        }

        public async Task<Memberships?> GetQrCodeByUserIdAsync(string userId)
        {
            return await _context.Memberships
                   .FirstOrDefaultAsync(m => m.UserId == userId);
        }
    }
}
