using Gym.Client.Interfaces;

namespace Gym.Client.Services
{
    public class UserService : IUserService
    {
        private HttpClient _http;   
        public UserService(HttpClient http)
        {
            _http = http;
        }

        public async Task<int> GetTotalAdminsAsync()
        {
            var result = await _http.GetFromJsonAsync<int>("api/User/total-admins");
            return result;
        }

        public async Task<int> GetTotalUserAsync()
        {
            var result = await _http.GetFromJsonAsync<int>("api/User/total-user");
            return result;
        }
    }
}
