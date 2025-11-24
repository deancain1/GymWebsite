using Gym.Client.DTOs;
using Gym.Client.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Gym.Client.Components.Dialog.AdminDialog
{
    public class EditAdminDialogBase : ComponentBase
    {
        [Inject] IUserService _userService { get; set; } = default!;
        [CascadingParameter] protected IMudDialogInstance MudDialog { get; set; } = default!;
        [Inject] ISnackbar Snackbar { get; set; } = default!;
        [Parameter] public string? UserID { get; set; }
        public UserDTO admin = new();

        public string? profileImagePreview;
        protected byte[]? profileImageBytes;
        public bool _showPassword = false;

        public void TogglePassword()
        {
            _showPassword = !_showPassword;
        }

        protected override async Task OnInitializedAsync()
        {
            admin.Role = "Admin";

            var result = await _userService.GetUserByIdAsync(UserID);

            if (result != null)
            {
                admin = result;

                if (admin.ProfilePicture != null)
                {
                    var base64 = Convert.ToBase64String(admin.ProfilePicture);
                    profileImagePreview = $"data:image/jpeg;base64,{base64}";
                }
            }
        }
        public async Task Update()
        {
            var result = await _userService.UpdateUserAsync(admin);
            if (result)
            {
                MudDialog.Close(DialogResult.Ok(admin));
                Snackbar.Add("Update Admin successfully.", Severity.Success);
            }
            else
            {
                MudDialog.Cancel();
            }
        }
 
        public void Cancel()
        {
            MudDialog.Cancel();
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

                    admin.ProfilePicture = profileImageBytes;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading profile picture: {ex.Message}");
            }
        }
    }
}
