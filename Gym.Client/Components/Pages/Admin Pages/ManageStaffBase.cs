using Gym.Client.Components.Dialog.AdminDialog;
using Gym.Client.DTOs;
using Gym.Client.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Gym.Client.Components.Pages.Admin_Pages
{
    public class ManageStaffBase : ComponentBase
    {
        [Inject] protected IAuthService _authService { get; set; } = default!;
        [Inject] protected IUserService _userService { get; set; } = default!;
        [Inject] protected IDialogService DialogService { get; set; } = default!;
        [Inject] protected ISnackbar Snackbar { get; set; } = default!;
        [CascadingParameter] protected IMudDialogInstance MudDialog { get; set; } = default!;
        protected SortMode _sortMode = SortMode.Multiple;

        protected UserDTO staffMember = new UserDTO();
        protected List<UserDTO> staff = new();
        protected HashSet<UserDTO> _selectedStaff = new();
        protected string? profileImagePreview;
        protected byte[]? profileImageBytes;

        public bool _showPassword = false;

        public void TogglePassword()
        {
            _showPassword = !_showPassword;
        }

        protected override async Task OnInitializedAsync()
        {
            staffMember.Role = "Staff";
            await LoadStaff();
        }

        private async Task LoadStaff()
        {
            staff = await _userService.GetAccountsByRoleAsync("Staff");
            StateHasChanged();
        }

        protected async Task AddStaff()
        {
            if (string.IsNullOrWhiteSpace(staffMember.FullName) ||
                 string.IsNullOrWhiteSpace(staffMember.PhoneNumber) ||
                 string.IsNullOrWhiteSpace(staffMember.Email) ||
                 string.IsNullOrWhiteSpace(staffMember.Gender) ||
                 string.IsNullOrWhiteSpace(staffMember.Address) ||
                 string.IsNullOrWhiteSpace(staffMember.Password) ||
                 string.IsNullOrWhiteSpace(staffMember.Role))
            {
                Snackbar.Add("Please fill in all required fields.", Severity.Warning);
                return;
            }
            if (!staffMember.Email.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase))
            {
                Snackbar.Add("Email must be a valid @gmail.com address.", Severity.Warning);
                return;
            }
            if (staffMember.PhoneNumber.Length != 11 || !staffMember.PhoneNumber.All(char.IsDigit))
            {
                Snackbar.Add("Phone number must be exactly 11 digits.", Severity.Warning);
                return;
            }
            if (staffMember.Password.Length < 8)
            {
                Snackbar.Add("Password must be at least 8 characters long and include uppercase, lowercase, number, and special character.", Severity.Warning);
                return;
            }
            var isSuccess = await _authService.CreateAccountAsync(staffMember);

            if (isSuccess)
            {
                await LoadStaff();
                Snackbar.Add("Staff added successfully!", Severity.Success);
                MudDialog?.Close(DialogResult.Ok(true));
                StateHasChanged();
            }
            else
            {
                Snackbar.Add("Failed to add Staff!", Severity.Error);
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


                staffMember.ProfilePicture = profileImageBytes;
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

            var dialogRef = await DialogService.ShowAsync<AddStaffDialog>("Add Staff", options);
            var result = await dialogRef.Result;


            if (!result.Canceled)
            {
                await LoadStaff();

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

            var dialogReference = await DialogService.ShowAsync<EditStaffDialog>("Edit Staff", parameters, options);
            var result = await dialogReference.Result;

            if (!result.Canceled && result.Data is UserDTO updateAdmin)
            {
                var index = staff.FindIndex(r => r.UserId == updateAdmin.UserId);
                if (index != -1)
                {
                    staff[index] = updateAdmin;
                    StateHasChanged();
                }
            }
        }

        protected async Task DeleteSelectedStaffAsync()
        {
            if (_selectedStaff == null || !_selectedStaff.Any())
                return;

            bool confirmed = (bool)await DialogService.ShowMessageBox(
                "Confirm Delete",
                $"Are you sure you want to delete {_selectedStaff.Count} selected staff?",
                yesText: "Yes", cancelText: "Cancel");

            if (confirmed)
            {
                foreach (var student in _selectedStaff)
                {
                    await _userService.DeleteUserAsync(student.UserId);
                }

                _selectedStaff.Clear();
                await LoadStaff();
                Snackbar.Add("Selected staff deleted successfully.", Severity.Success);
            }
        }
    }
}

