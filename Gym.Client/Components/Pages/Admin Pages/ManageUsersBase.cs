using Gym.Client.Components.Dialog.AdminDialog;
using Gym.Client.DTOs;
using Gym.Client.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Gym.Client.Components.Pages.Admin_Pages
{
    public class ManageUsersBase : ComponentBase
    {
        [Inject] protected IAuthService _authService { get; set; } = default!;
        [Inject] protected IUserService _userService { get; set; } = default!;
        [Inject] protected IDialogService DialogService { get; set; } = default!;
        [Inject] protected ISnackbar Snackbar { get; set; } = default!;
        [CascadingParameter] protected IMudDialogInstance MudDialog { get; set; } = default!;

        protected SortMode _sortMode = SortMode.Multiple;
        protected UserDTO user = new UserDTO();
        protected List<UserDTO> users = new();
        protected HashSet<UserDTO> _selectedUser = new();
        protected string? profileImagePreview;
        protected byte[]? profileImageBytes;
        protected string searchText = "";
        protected bool _showPassword = false;

        protected void TogglePassword()
        {
            _showPassword = !_showPassword;
        }

        private async Task LoadUsers()
        {
            users = await _userService.GetAccountsByRoleAsync("User");
            StateHasChanged();
        }
        protected override async Task OnInitializedAsync()
        {
            user.Role = "User";
            await LoadUsers();
        }
        protected List<UserDTO> filteredUsers =>
         string.IsNullOrWhiteSpace(searchText)
             ? users
             : users.Where(m =>
                     (m.FullName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false))
                    .ToList();
        protected async Task AddUser()
        {
            if (string.IsNullOrWhiteSpace(user.FullName) ||
                 string.IsNullOrWhiteSpace(user.PhoneNumber) ||
                 string.IsNullOrWhiteSpace(user.Email) ||
                 string.IsNullOrWhiteSpace(user.Gender) ||
                 string.IsNullOrWhiteSpace(user.Address) ||
                 string.IsNullOrWhiteSpace(user.Password) ||
                 string.IsNullOrWhiteSpace(user.Role))
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
            var isSuccess = await _authService.CreateAccountAsync(user);

            if (isSuccess)
            {
                await LoadUsers();
                Snackbar.Add("User added successfully!", Severity.Success);
                MudDialog?.Close(DialogResult.Ok(true));
                StateHasChanged();
            }
            else
            {
                 Snackbar.Add("Failed to add User!", Severity.Error);
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
        protected async Task OpenDialogAsync()
        {
            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                MaxWidth = MaxWidth.Medium,
                FullWidth = true
            };

            var dialogRef = await DialogService.ShowAsync<AddUserDialog>("Add user", options);
            var result = await dialogRef.Result;


            if (!result.Canceled)
            {
                await LoadUsers();
              
            }
        }

        protected void Cancel() => MudDialog?.Cancel();
        protected async Task OpenEditDialog(string UserID)
        {
            var parameters = new DialogParameters { { "UserID", UserID } };
            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                MaxWidth = MaxWidth.Medium,
                FullWidth = true
            };

            var dialogReference = await DialogService.ShowAsync<EditUserDialog>("Edit User", parameters, options);
            var result = await dialogReference.Result;

            if (!result.Canceled && result.Data is UserDTO updateUser)
            {
                var index = users.FindIndex(r => r.UserId == updateUser.UserId);
                if (index != -1)
                {
                    users[index] = updateUser;
                    StateHasChanged();
                }
            }
        }

        protected async Task DeleteSelectedUsersAsync()
        {
            if (_selectedUser == null || !_selectedUser.Any())
                return;

            bool confirmed = (bool)await DialogService.ShowMessageBox(
                "Confirm Delete",
                $"Are you sure you want to delete {_selectedUser.Count} selected user(s)?",
                yesText: "Yes", cancelText: "Cancel");

            if (confirmed)
            {
                foreach (var student in _selectedUser)
                {
                    await _userService.DeleteUserAsync(student.UserId);
                }

                _selectedUser.Clear();
                await LoadUsers();
                Snackbar.Add("Selected user deleted successfully.", Severity.Success);
            }
        }
        protected string? GetProfileImage(UserDTO user)
        {
            if (user?.ProfilePicture != null && user.ProfilePicture.Length > 0)
            {
                
                return $"data:image/png;base64,{Convert.ToBase64String(user.ProfilePicture)}";
            }
            return null;
        }

    }
}
