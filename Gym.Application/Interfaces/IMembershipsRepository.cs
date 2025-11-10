using Gym.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Application.Interfaces
{
    public interface IMembershipsRepository
    {
        Task CreateMembershipAsync(Memberships memberships);
        Task<List<Memberships>> GetAllMembershipsAsync();
        Task<Memberships?> GetMemberByIDAsync(int memberID);
        Task UpdateMembershipStatusAsync(Memberships memberships);
        Task<int> GetTotalMembershipsAsync();
        Task<Dictionary<string, int>> GetMembershipsPerMonthAsync();
        Task<Dictionary<string, int>> GetExpiredMembershipsPerMonthAsync();
        Task<Dictionary<string, int>> GetMembershipPlanCountsAsync();
        Task<Memberships?> GetQrCodeByTokenAsync(string userId);
        Task DeleteMembershipsAsync (Memberships memberships);

    }
}
