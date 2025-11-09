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
        Task<List<MembershipsPerMonthDTO>> GetMembershipsPerMonthAsync();
        Task<List<MembershipsPerMonthDTO>> GetExpiredMembershipsPerMonthAsync();
        Task<List<MembershipPlanDTO>> GetMembershipPlanCountsAsync();
        Task<MembershipDTO?> GetQrCodeByTokenAsync();

    }
}


