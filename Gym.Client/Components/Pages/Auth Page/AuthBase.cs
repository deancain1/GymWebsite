using Blazored.LocalStorage;
using Gym.Client.DTOs;
using Gym.Client.Interfaces;
using Gym.Client.Security;
using Gym.Client.Services;
using Microsoft.AspNetCore.Components;
using static System.Net.WebRequestMethods;

namespace Gym.Client.Components.Pages.Auth_Page
{
    public class AuthBase : ComponentBase
    {

        [Inject] HttpClient _http { get; set; } = default!;
        [Inject] IUserService _userService { get; set; } = default!;
        [Inject] CustomAuthStateProvider AuthStateProvider { get; set; } = default!;
        [Inject] ILocalStorageService _localStorageService { get; set; } = default!;
        [Inject] NavigationManager Navigation { get; set; } = default!;

        public UserDTO user = new UserDTO();
        public LoginRequestDTO loginModel = new();
        [Parameter]
        [SupplyParameterFromQuery]
        public string? returnUrl { get; set; }
    
        protected override void OnInitialized()
        {
            user.Role = "User";
        }

        public async Task HandleRegister()
        {
            var registerAccount = await _userService.CreateAccountAsync(user);

            if (registerAccount)
            {
                Navigation.NavigateTo("/");
            }
            else
            {
                Console.WriteLine("Registration failed");
            }
        }

        public async Task HandleLogin()
        {
            var response = await _userService.LoginAsync(loginModel);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Login failed");
                return;
            }

            var result = await response.Content.ReadFromJsonAsync<AuthResponseDTO>();
            if (result == null || string.IsNullOrEmpty(result.Token))
            {
                Console.WriteLine("Invalid login response");
                return;
            }

            var token = result.Token;
            var role = result.Role;

            await _localStorageService.SetItemAsStringAsync("authToken", token);
            await _localStorageService.SetItemAsStringAsync("userRole", role);

            AuthStateProvider.NotifyUserAuthentication(token);



            if (!string.IsNullOrEmpty(returnUrl))
            {

                Navigation.NavigateTo("/");
            }
            else if (role == "Admin")
            {
                Navigation.NavigateTo("/adminhome", true);
            }
            else if (role == "User")
            {
                Navigation.NavigateTo("/userhome");
            }
            else
            {
                Navigation.NavigateTo("/");
            }

        }
    }
}
