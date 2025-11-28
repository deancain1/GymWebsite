using Blazored.LocalStorage;
using Gym.Client.DTOs;
using Gym.Client.Interfaces;
using System.Data;
using static System.Net.WebRequestMethods;

namespace Gym.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;
      

        public AuthService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _http = httpClient;
         
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
            return await _http.PostAsJsonAsync("api/Auth/login", loginModel);
        }
    }
}