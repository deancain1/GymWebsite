using Gym.Client.DTOs;
using Gym.Client.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Gym.Client.Components.Dialog.AdminDialog
{
    public class EditUsersDialogBase : ComponentBase
    {
        [Inject] IUserService _userService { get; set; } = default!;
        [CascadingParameter] protected IMudDialogInstance MudDialog { get; set; } = default!;
        [Inject] protected ISnackbar Snackbar { get; set; } = default!;
        [Parameter] public string? UserID { get; set; }
        public UserDTO user = new();

        public string? profileImagePreview;
        protected byte[]? profileImageBytes;
        protected override async Task OnInitializedAsync()
        {
            user.Role = "User";
            var result = await _userService.GetUserByIdAsync(UserID);

            if (result != null)
            {
                user = result;
                
                if (user.ProfilePicture != null)
                {
                    var base64 = Convert.ToBase64String(user.ProfilePicture);
                    profileImagePreview = $"data:image/jpeg;base64,{base64}";
                }
            }
        }
        public async Task Update()
        {
            var result = await _userService.UpdateUserAsync(user);
            if (result)
            {
                MudDialog.Close(DialogResult.Ok(user));
                Snackbar.Add("Update User successfully.", Severity.Success);
            }
            else
            {
                MudDialog.Cancel();
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
        public void Cancel()
        {
            MudDialog.Cancel();
        }
    }
}
