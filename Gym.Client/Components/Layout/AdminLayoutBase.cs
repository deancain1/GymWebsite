using Gym.Client.Components.Dialog.UserDialog;
using Gym.Client.Security;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Gym.Client.Components.Layout
{
    public class AdminLayoutBase : LayoutComponentBase
    {
        [Inject] IDialogService DialogService { get; set; } = default!;
        [Inject] CustomAuthStateProvider AuthStateProvider { get; set; } = default!;
        [Inject] NavigationManager Navigation { get; set; } = default!;

        public bool _drawerOpen = true;
        public bool _initialized;
        public bool isAdmin;
        public bool isStaff;
        public string? fullName;
        public void DrawerToggle() => _drawerOpen = !_drawerOpen;
        public bool _isDark = true;
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            fullName = user.FindFirst("FullName")?.Value ?? "Admin, Staff";
            isAdmin = user.IsInRole("Admin");
            isStaff = user.IsInRole("Staff");
            _initialized = true;
            await Task.Delay(5000);
        }
        public async Task Logout()
        {
            await AuthStateProvider.MarkUserAsLoggedOutAsync();
            Navigation.NavigateTo("/", true);
        }

        public async Task OpenScannerDialog()
        {
            var options = new DialogOptions() { MaxWidth = MaxWidth.Small, FullWidth = true };
            await DialogService.ShowAsync<ScannerDialog>("", options);
        }
        
        public MudTheme _lightTheme = new MudTheme()
        {
            PaletteLight = new PaletteLight()
            {
                Primary = "#000000",
                Background = "#FFFFFF",
                Surface = "#F5F5F5",
                TextPrimary = "#000000",
                DrawerBackground = "#FFFFFF"
            }
        };


        public MudTheme _darkTheme = new MudTheme()
        {
            PaletteDark = new PaletteDark()
            {
                Primary = "#000000",
                Background = "#121212",
                Surface = "#1E1E1E",
                TextPrimary = "#FFFFFF",
                DrawerBackground = "#1E1E1E",
                DrawerText = "#FFFFFF",
              
            }
        };


        public void ToggleTheme()
        {
            _isDark = !_isDark;
        }
    }
}
