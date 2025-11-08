using Blazored.LocalStorage;
using Gym.Client.DTOs;
using Gym.Client.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using static System.Net.WebRequestMethods;

namespace Gym.Client.Components.Dialog.UserDialog
{
    public class EditProfileDialogBase : ComponentBase
    {

        [Inject] HttpClient _http { get; set; } = default!;
        [Inject] IUserService _userService { get; set; } = default!;
        [Inject] ILocalStorageService LocalStorage { get; set; } = default!;
        [CascadingParameter] protected IMudDialogInstance MudDialog { get; set; } = default!;

        public UserDTO user = new();
        public byte[]? ImageBytes;
        public string? ProfilePreview;

        protected override async Task OnInitializedAsync()
        {
            var token = await LocalStorage.GetItemAsStringAsync("authToken");

            if (!string.IsNullOrWhiteSpace(token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }


            user = await _userService.GetCurrentUserByTokenAsync() ?? new UserDTO();

            if (user.ProfilePicture != null)
            {
                var base64 = Convert.ToBase64String(user.ProfilePicture);
                ProfilePreview = $"data:image/jpeg;base64,{base64}";
            }
        }

        public async Task HandleProfilePicUpload(InputFileChangeEventArgs e)
        {
            var file = e.File;
            var buffer = new byte[file.Size];
            await file.OpenReadStream(2 * 1024 * 1024).ReadExactlyAsync(buffer);

            ImageBytes = buffer;
            user.ProfilePicture = buffer;

            ProfilePreview = $"data:{file.ContentType};base64,{Convert.ToBase64String(buffer)}";
        }

        public async Task Save()
        {
            var userInfo = await _userService.UpdateUserAsync(user);
            MudDialog.Close(DialogResult.Ok(user));
        }
        public void Cancel()
        {
            MudDialog.Cancel();
        }
    }
}
