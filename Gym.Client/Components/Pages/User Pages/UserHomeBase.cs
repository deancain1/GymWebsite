using Blazored.LocalStorage;
using Gym.Client.Components.Dialog.UserDialog;
using Gym.Client.DTOs;
using Gym.Client.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Gym.Client.Components.Pages.User_Pages
{
    public class UserHomeBase : ComponentBase
    {
        [Inject] protected HttpClient _http { get; set; } = default!;
        [Inject] protected IUserService _userService { get; set; } = default!;
        [Inject] protected IMembershipService _membershipService { get; set; } = default!;
        [Inject] protected IAttendanceService _attendanceService { get; set; } = default!;
        [Inject] protected IDialogService DialogService { get; set; } = default!;
        [Inject] protected ILocalStorageService _localStorageService { get; set; } = default!;

        public List<AttendanceLogDTO> Attendances = new();
        public UserDTO user = new();
        public string? userQrCode;
        public string? profileImagePreview;
        protected byte[]? profileImageBytes;
        protected override async Task OnInitializedAsync()
        {
            try
            {

                var token = await _localStorageService.GetItemAsStringAsync("authToken");

                if (!string.IsNullOrEmpty(token))
                {

                    _http.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }
                else
                {
                    Console.WriteLine("No token found in local storage.");
                    Attendances = new List<AttendanceLogDTO>();
                    return;
                }

                user = await _userService.GetCurrentUserByTokenAsync() ?? new UserDTO();
                if (user != null)
                {
                    if (user.ProfilePicture != null)
                    {
                        var base64 = Convert.ToBase64String(user.ProfilePicture);
                        profileImagePreview = $"data:image/jpeg;base64,{base64}";
                    }
                }
                var membership = await _membershipService.GetQrCodeByTokenAsync() ?? new MembershipDTO();
                if (membership?.QRCode != null)
                {

                    userQrCode = $"data:image/png;base64,{membership.QRCode}";
                }
                Attendances = await _attendanceService.GetAttendanceByTokenAsync()
                              ?? new List<AttendanceLogDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching attendance: " + ex.Message);
                Attendances = new List<AttendanceLogDTO>();
                user = new UserDTO();
            }
        }
        protected async Task HandleProfilePicUpload(InputFileChangeEventArgs e)
        {
            try
            {
                var file = e.File;

                if (file != null)
                {
                    var buffer = new byte[file.Size];
                    await file.OpenReadStream(maxAllowedSize: 2 * 1024 * 1024).ReadExactlyAsync(buffer);

                    profileImageBytes = buffer;
                    profileImagePreview = $"data:{file.ContentType};base64,{Convert.ToBase64String(buffer)}";

                    user.ProfilePicture = profileImageBytes;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading profile picture: {ex.Message}");
            }
        }
        public async Task OpenEditDialog()
        {
            var dialog = DialogService.Show<EditProfileDialog>("");
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                await OnInitializedAsync();
                StateHasChanged();
            }
        }
    }
}
