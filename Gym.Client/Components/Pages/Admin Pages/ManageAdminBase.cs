using Gym.Client.Components.Dialog.AdminDialog;
using Gym.Client.DTOs;
using Gym.Client.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Gym.Client.Components.Pages.Admin_Pages
{
    public class ManageAdminBase : ComponentBase
    {
        [Inject] protected IUserService _userService { get; set; } = default!;
        [Inject] protected IDialogService DialogService { get; set; } = default!;
        [Inject] protected ISnackbar Snackbar { get; set; } = default!;
        [CascadingParameter] protected IMudDialogInstance MudDialog { get; set; } = default!;
        protected SortMode _sortMode = SortMode.Multiple;

        protected UserDTO user = new UserDTO();
        protected List<UserDTO> admins = new();
        protected HashSet<UserDTO> _selectedAdmin = new();
        protected string? profileImagePreview;
        protected byte[]? profileImageBytes;
        protected override async Task OnInitializedAsync()
        {
            user.Role = "Admin";
            await LoadAdmins();
        }

        private async Task LoadAdmins()
        {
            admins = await _userService.GetAccountsByRoleAsync("Admin");
            StateHasChanged();
        }
       
        protected async Task AddAdmin()
        {
            if (string.IsNullOrWhiteSpace(user.FullName) ||
                 string.IsNullOrWhiteSpace(user.PhoneNumber) ||
                 string.IsNullOrWhiteSpace(user.Email) ||
                 string.IsNullOrWhiteSpace(user.Gender) ||
                 string.IsNullOrWhiteSpace(user.Address) ||
                 string.IsNullOrWhiteSpace(user.Password) ||
                 string.IsNullOrWhiteSpace(user.Role))
            {
                Console.WriteLine("Please fill in all required fields.");
                return;
            }

            var isSuccess = await _userService.CreateAccountAsync(user);

            if (isSuccess)
            {
                await LoadAdmins();
                Snackbar.Add("Admin added successfully!", Severity.Success);
                MudDialog?.Close(DialogResult.Ok(true));
                StateHasChanged();
            }
            else
            {
                Console.WriteLine("Failed to add user.");
            }
        }

        protected async Task HandleProfilePicUpload(InputFileChangeEventArgs e)
        {
            var file = e.File;

            if (file != null)
            {
                var buffer = new byte[file.Size];
                await file.OpenReadStream(maxAllowedSize: 5 * 1024 * 1024).ReadExactlyAsync(buffer);

                profileImageBytes = buffer;
                profileImagePreview = $"data:{file.ContentType};base64,{Convert.ToBase64String(buffer)}";


                user.ProfilePicture = profileImageBytes;
            }
        }
        public async Task OpenDialogAsync()
        {
            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                MaxWidth = MaxWidth.Medium,
                FullWidth = true
            };

            var dialogRef = await DialogService.ShowAsync<AddAdminDIalog>("Add Admin", options);
            var result = await dialogRef.Result;

            
            if (!result.Canceled)
            {
                await LoadAdmins();
              
            }
        }

        protected void Cancel() => MudDialog?.Cancel();
        public async Task OpenEditDialog(string UserID)
        {
            var parameters = new DialogParameters { { "UserID", UserID } };
            var options = new DialogOptions
            {
                CloseButton = true,
                MaxWidth = MaxWidth.Medium,
                FullWidth = true
            };

            var dialogReference = await DialogService.ShowAsync<EditAdminDialog>("Edit Admin", parameters, options);
            var result = await dialogReference.Result;

            if (!result.Canceled && result.Data is UserDTO updateAdmin)
            {
                var index = admins.FindIndex(r => r.UserId == updateAdmin.UserId);
                if (index != -1)
                {
                    admins[index] = updateAdmin;
                    StateHasChanged();
                }
            }
        }
    
        protected async Task DeleteSelectedAdminsAsync()
        {
            if (_selectedAdmin == null || !_selectedAdmin.Any())
                return;

            bool confirmed = (bool)await DialogService.ShowMessageBox(
                "Confirm Delete",
                $"Are you sure you want to delete {_selectedAdmin.Count} selected admin(s)?",
                yesText: "Yes", cancelText: "Cancel");

            if (confirmed)
            {
                foreach (var student in _selectedAdmin)
                {
                    await _userService.DeleteUserAsync(student.UserId);
                }

                _selectedAdmin.Clear();
                await LoadAdmins();
                Snackbar.Add("Selected admins deleted successfully.", Severity.Success);
            }
        }
    }
}

