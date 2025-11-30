using Gym.Client.DTOs;
using Gym.Client.Interfaces;
using Gym.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using static System.Net.WebRequestMethods;

namespace Gym.Client.Components.Pages.Admin_Pages
{
    public class MembershipsBase : ComponentBase
    {
        [Inject] protected IJSRuntime JS { get; set; } = default!;
        [Inject] protected HttpClient Http { get; set; } = default!;
        [Inject] protected IMembershipService _membershipService { get; set; } = default!;
        [Inject] protected IDialogService DialogService { get; set; } = default!;
        [Inject] protected ISnackbar Snackbar { get; set; } = default!;

        protected List<MembershipDTO> members = new();
        protected SortMode _sortMode = SortMode.Multiple;
        protected HashSet<MembershipDTO> selectedMembers = new();

        protected string searchText = "";
        protected string selectedStatus = string.Empty;
        
        protected List<MembershipDTO> filteredMembers =>
            string.IsNullOrWhiteSpace(searchText)
                ? members
                : members.Where(m =>
                        (m.FullName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false))
                       .ToList();

        protected override async Task OnInitializedAsync()
        {
            await LoadMembers();
        }

        protected async Task LoadMembers()
        {
            members = await _membershipService.GetAllMembershipsAsync();
        }

        protected async Task UpdateSelectedStatusAsync(string status)
        {
            foreach (var member in selectedMembers)
            {
                var isUpdated = await _membershipService.UpdateMembershipStatusAsync(member.MemberID, status);
                if (isUpdated)
                {
                    Snackbar.Add($"{member.FullName} membership application {status}", Severity.Success);
                }
                else
                {
                    Snackbar.Add($"Failed to update {member.FullName}", Severity.Error);
                }
            }

            await LoadMembers();
            selectedMembers.Clear();
        }
        protected async Task GenerateSelectedPdfsAsync()
        {
            foreach (var member in selectedMembers)
            {
                await JS.InvokeVoidAsync("generatePdf",
                    member.FullName,
                    member.Email,
                    member.PhoneNumber,
                    member.Status,
                    member.AppliedDate.ToString("dd/MM/yyyy"),
                    member.QRCode
                );
            }
        }
        protected async Task DeleteSelectedMembersAsync()
        {
            if (selectedMembers == null || !selectedMembers.Any())
                return;

            bool confirmed = (bool)await DialogService.ShowMessageBox(
                "Confirm Delete",
                $"Are you sure you want to delete {selectedMembers.Count} selected members(s)?",
                yesText: "Yes", cancelText: "Cancel");

            if (confirmed)
            {
                foreach (var student in selectedMembers)
                {
                    await _membershipService.DeleteMembershipsAsync(student.MemberID);
                }

                selectedMembers.Clear();
                await LoadMembers();
                Snackbar.Add("Selected user deleted successfully.", Severity.Success);
            }
        }
        public Color GetStatusColor(string status)
        {
            return status switch
            {
                "Accepted" => Color.Success,
                "Pending" => Color.Warning,
                "Rejected" => Color.Error,
                _ => Color.Default
            };
        }

    }
}
