using Gym.Client.DTOs;
using Gym.Client.Interfaces;
using Gym.Client.Security;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Security.Claims;

namespace Gym.Client.Components.Dialog.UserDialog
{
    public class ApplicationFormBase : ComponentBase
    {
        [Inject] protected CustomAuthStateProvider AuthStateProvider { get; set; } = default!;
        [Inject] protected IMembershipService _membershipService { get; set; } = default!;
        [Inject] public ISnackbar Snackbar { get; set; } = default!;
        [CascadingParameter] IMudDialogInstance MudDialog { get; set; } = default!;

        public MembershipDTO member = new();

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity?.IsAuthenticated == true)
            {
                member.FullName = user.Identity.Name ?? string.Empty;
                member.Email = user.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
                member.PhoneNumber = user.FindFirst("PhoneNumber")?.Value ?? string.Empty;
                member.Address = user.FindFirst("Address")?.Value ?? string.Empty;
                member.UserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            }
        }

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        protected async Task CreateMembership()
        {
            if (string.IsNullOrWhiteSpace(member.FullName) ||
                string.IsNullOrWhiteSpace(member.Email) ||
                string.IsNullOrWhiteSpace(member.PhoneNumber) ||
                string.IsNullOrWhiteSpace(member.Address))
            {
                Snackbar.Add("Please fill in all fields.", Severity.Warning);
                return;
            }

            bool isAdded = await _membershipService.CreateMembershipAsync(member);

            if (isAdded)
            {
                Snackbar.Add($"Applying Membership success. Duration: {member.DurationMonths} month(s).", Severity.Success);
                member = new MembershipDTO();
                StateHasChanged();
                MudDialog.Close(DialogResult.Ok(true));
            }
            else
            {
                Snackbar.Add("Failed to create membership.", Severity.Error);
            }
        }
    }
}
