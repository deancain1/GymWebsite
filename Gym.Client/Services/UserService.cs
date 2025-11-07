using Gym.Client.DTOs;
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
        public async Task<bool> CreateAccountAsync(UserDTO userDTO)
        {
            var result = await _http.PostAsJsonAsync("api/Auth/register", userDTO);
            if (result.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        public async Task<HttpResponseMessage> LoginAsync(LoginRequestDTO loginModel)
        {
            return await _http.PostAsJsonAsync("api/auth/login", loginModel);
        }
        public async Task<List<UserDTO>> GetAccountsByRoleAsync(string roleName)
        {
            var result = await _http.GetFromJsonAsync<List<UserDTO>>($"api/User/by-role/{roleName}");
            return result ?? new List<UserDTO>();
        }
        public async Task<UserDTO?> GetUserByIdAsync(string userId)
        {
            try
            {
                return await _http.GetFromJsonAsync<UserDTO>($"api/User/{userId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user: {ex.Message}");
                return null;
            }
        }
        public async Task<bool> UpdateUserAsync(UserDTO user)
        {
            var response = await _http.PutAsJsonAsync($"api/User/{user.UserId}", user);
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteUserAsync(string UserId)
        {
            var response = await _http.DeleteAsync($"api/User/{UserId}");
            return response.IsSuccessStatusCode;
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

        public async Task<UserDTO?> GetCurrentUserByTokenAsync()
        {
            return await _http.GetFromJsonAsync<UserDTO>("api/User/user-info");
        }

       
    }
}
