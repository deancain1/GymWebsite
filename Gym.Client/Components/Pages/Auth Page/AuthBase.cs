using Blazored.LocalStorage;
using Gym.Client.DTOs;
using Gym.Client.Interfaces;
using Gym.Client.Security;
using Gym.Client.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.IdentityModel.Tokens.Jwt;
using static System.Net.WebRequestMethods;

namespace Gym.Client.Components.Pages.Auth_Page
{
    public class AuthBase : ComponentBase
    {
        [Inject] AuthTokenProvider AuthTokenProvider { get; set; } = default!;
        [Inject] HttpClient _http { get; set; } = default!;
        [Inject] IAuthService _authService { get; set; } = default!;
        [Inject] protected ISnackbar Snackbar { get; set; } = default!;
        [Inject] CustomAuthStateProvider AuthStateProvider { get; set; } = default!;
        [Inject] ILocalStorageService _localStorageService { get; set; } = default!;
        [Inject] NavigationManager Navigation { get; set; } = default!;

        public UserDTO user = new UserDTO();
        public LoginRequestDTO loginModel = new();
        [Parameter]
        [SupplyParameterFromQuery]
        public string? returnUrl { get; set; }

        // For Password
        public bool _showPassword = false;
        public int step = 1;
        public string Email { get; set; }
        public string OtpCode { get; set; }

        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        public string verifiedOtp;
       
        public bool isResendDisabled = false;
        public int countdown = 30;
        protected override void OnInitialized()
        {
            user.Role = "User";
        }

        public async Task HandleRegister()
        {
            if (string.IsNullOrWhiteSpace(user.FullName) ||
                string.IsNullOrWhiteSpace(user.Email) ||
                string.IsNullOrWhiteSpace(user.PhoneNumber) ||
                string.IsNullOrWhiteSpace(user.Gender) ||
                string.IsNullOrWhiteSpace(user.Address) ||
                user.DateOfBirth == null ||
                string.IsNullOrWhiteSpace(user.Password))

            {
                Snackbar.Add("Please fill in all required fields.", Severity.Warning);
                return;
            }
            if (!user.Email.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase))
            {
                Snackbar.Add("Email must be a valid @gmail.com address.", Severity.Warning);
                return;
            }
            if (user.PhoneNumber.Length != 11 || !user.PhoneNumber.All(char.IsDigit))
            {
                Snackbar.Add("Phone number must be exactly 11 digits.", Severity.Warning);
                return;
            }
            if (user.Password.Length < 8)
            {
                Snackbar.Add("Password must be at least 8 characters long and include uppercase, lowercase, number, and special character.", Severity.Warning);
                return;
            }

            var registerAccount = await _authService.CreateAccountAsync(user);

            if (registerAccount)
            {
                Navigation.NavigateTo(returnUrl ?? "/", true);
            }

            else
            {
                Snackbar.Add("Registration failed. Please try again.", Severity.Error);
            }
        }


        public async Task HandleLogin()
        {
            if (string.IsNullOrWhiteSpace(loginModel.Email) ||
                string.IsNullOrWhiteSpace(loginModel.Password))
            {
                Snackbar.Add("Please fill in all required fields.", Severity.Warning);
                return;
            }

            var response = await _authService.LoginAsync(loginModel);

            if (!response.IsSuccessStatusCode)
            {
                Snackbar.Add("Incorrect email or password.", Severity.Error);
                return;
            }

            var result = await response.Content.ReadFromJsonAsync<AuthResponseDTO>();
            if (result == null || string.IsNullOrEmpty(result.Token))
            {
                Console.WriteLine("Invalid login response");
                return;
            }

            var token = result.Token;
            var refreshtoken = result.RefreshToken;
            var role = result.Role;

    
            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var expiry = jwtToken.ValidTo; 


            AuthTokenProvider.SetTokens(token, refreshtoken, expiry);
         
            await _localStorageService.SetItemAsStringAsync("authToken", token);
            await _localStorageService.SetItemAsStringAsync("refreshToken", refreshtoken);
            await _localStorageService.SetItemAsStringAsync("userRole", role);

            AuthStateProvider.NotifyUserAuthentication(token);

            if (!string.IsNullOrEmpty(returnUrl))
            {
                Navigation.NavigateTo(returnUrl, true);
            }
            else if (role == "Admin" || role == "Staff")
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

        public void TogglePassword()
        {
            _showPassword = !_showPassword;
        }

        public async Task SendOtp()
        {
            var response = await _http.PostAsJsonAsync("api/auth/forgot-password", new
            {
                Email = this.Email
            });

            if (response.IsSuccessStatusCode)
            {
                step = 2;
                await DisableResendButton();

            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine(error);
            }
        }

       
        public async Task VerifyOtp()
        {
            var response = await _http.PostAsJsonAsync("api/auth/verify-otp", new
            {
                Email = this.Email,
                OtpCode = this.OtpCode
            });

            if (response.IsSuccessStatusCode)
            {
                verifiedOtp = OtpCode;
                step = 3;
            }
            else
            {
              
            }
        }

       
        public async Task ResetPassword()
        {
            if (NewPassword != ConfirmPassword)
            {
                return;
            }

            var response = await _http.PostAsJsonAsync("api/auth/reset-password", new
            {
                Email = this.Email,
                OtpCode = verifiedOtp,
                NewPassword = this.NewPassword


            });

            if (response.IsSuccessStatusCode)
            {
                Navigation.NavigateTo("/login");
            }
            else
            {
               Snackbar.Add("Failed to reset password", Severity.Error);
            }
        }
        public async Task DisableResendButton()
        {
            isResendDisabled = true;
            countdown = 30;
            while (countdown > 0)
            {
                await Task.Delay(1000);
                countdown--;
                await InvokeAsync(StateHasChanged);
            }

            isResendDisabled = false;
           
        }

    }

}
