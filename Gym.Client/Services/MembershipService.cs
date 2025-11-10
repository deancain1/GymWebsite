using Blazored.LocalStorage;
using Gym.Client.DTOs;
using Gym.Client.Interfaces;
using System.Net.Http.Headers;

namespace Gym.Client.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localstorage;
        public MembershipService(HttpClient http, ILocalStorageService localStorage)
        {
            _http = http;
            _localstorage = localStorage;

        }
        public async Task<bool> CreateMembershipAsync(MembershipDTO membershipDTO)
        {
            var token = await _localstorage.GetItemAsync<string>("authToken");
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer" , token);

            var result = await _http.PostAsJsonAsync("api/Membership/create-membership", membershipDTO);
            return result.IsSuccessStatusCode;
        }

        public async Task<List<MembershipDTO>> GetAllMembershipsAsync()
        {
            return await _http.GetFromJsonAsync<List<MembershipDTO>>("api/Membership/get-all-members") ?? new();
        }

        public async Task<int> GetTotalMembershipsAsync()
        {
            return await _http.GetFromJsonAsync<int>("api/Membership/get-total-memberships"); 
        }

        public async Task<List<MembershipDTO>> GetUserMembershipsAsync()
        {
            var response = await _http.GetFromJsonAsync<List<MembershipDTO>>($"api/Membership/my-memberships");
            return response ?? new List<MembershipDTO>();
        }

        public async Task<bool> UpdateMembershipStatusAsync(int memberId, string status)
        {

            var result = await _http.PutAsJsonAsync($"api/Membership/update-status/{memberId}", status);
            return result.IsSuccessStatusCode;
        }
        public async Task<List<MembershipsPerMonthDTO>> GetMembershipsPerMonthAsync()
        {
         

            var response = await _http.GetFromJsonAsync<List<MembershipsPerMonthDTO>>("api/Membership/memberships-per-month");
            return response ?? new List<MembershipsPerMonthDTO>();
        }

        public async Task<List<MembershipsPerMonthDTO>> GetExpiredMembershipsPerMonthAsync()
        {
           

            var response = await _http.GetFromJsonAsync<List<MembershipsPerMonthDTO>>("api/Membership/expired-memberships");
            return response ?? new List<MembershipsPerMonthDTO>();
        }
        public async Task<List<MembershipPlanDTO>> GetMembershipPlanCountsAsync()
        {
            var response = await _http.GetFromJsonAsync<List<MembershipPlanDTO>>("api/Membership/plan-counts")
                            ?? new List<MembershipPlanDTO>();
            return response;
        }

        public async Task<MembershipDTO?> GetQrCodeByTokenAsync()
        {
            return await _http.GetFromJsonAsync<MembershipDTO>("api/Membership/user-qrcode");
        }
        public async Task<bool> DeleteMembershipsAsync(int MemberID)
        {
            var response = await _http.DeleteAsync($"api/Membership/{MemberID}");
            return response.IsSuccessStatusCode;
        }
    }
}
