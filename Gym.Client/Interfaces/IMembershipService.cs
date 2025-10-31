using Gym.Client.DTOs;

namespace Gym.Client.Interfaces
{
    public interface IMembershipService
    {
        Task<bool> CreateMembershipAsync(MembershipDTO membershipDTO);
        Task<List<MembershipDTO>> GetAllMembershipsAsync();
        Task<bool> UpdateMembershipStatusAsync(int memberId, string status);
        Task<List<MembershipDTO>> GetUserMembershipsAsync();
        Task <int> GetTotalMembershipsAsync();
        Task<List<MembershipsPerMonthDTO>> GetMonthlyMembershipsAsync();
        Task<List<MembershipsPerMonthDTO>> GetMonthlyExpiredAsync();
        Task<List<MembershipPlanDTO>> GetPlanCountsAsync();

    }
}
