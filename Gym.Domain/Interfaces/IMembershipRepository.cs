using Gym.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Domain.Interfaces
{
    public interface  IMembershipRepository
    {
        Task CreateMembershipAsync(Memberships memberships);
        Task<List<Memberships>> GetAllMembershipsAsync();
        Task<Memberships?> GetMemberByIDAsync(int memberID);
        Task UpdateStatusAsync(Memberships memberships);
        Task<List<Memberships>> GetMembershipsByUserIdAsync(string userId);
        Task<List<Memberships>> GetActiveMembershipsAsync();
        Task<int> GetTotalMembershipsAsync();
        Task<Dictionary<string, int>> GetMembershipsPerMonthAsync();
        Task<Dictionary<string, int>> GetExpiredMembershipsPerMonthAsync();
        Task<Dictionary<string, int>> GetMembershipPlanCountsAsync();

    }
}
