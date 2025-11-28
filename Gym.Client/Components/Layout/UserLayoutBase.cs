using Gym.Client.Security;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Gym.Client.Components.Layout
{
    public class UserLayoutBase : LayoutComponentBase
    {
        [Inject] CustomAuthStateProvider AuthStateProvider { get; set; } = default!;
        [Inject] NavigationManager Navigation { get; set; } = default!;

        public bool _drawerOpen = true;
        public bool isUser;
        public string? fullName;
        public bool _initialized;
        public void DrawerToggle() => _drawerOpen = !_drawerOpen;
        public bool _isDark = true;
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            fullName = user.FindFirst("FullName")?.Value ?? "User";
            isUser = user.IsInRole("User");
            await Task.Delay(5000);
            _initialized = true;
        }

        public async Task Logout()
        {
            await AuthStateProvider.MarkUserAsLoggedOutAsync();
            Navigation.NavigateTo("/", true);
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
                DrawerText = "#FFFFFF"
            }
        };
        public void ToggleTheme()
        {
            _isDark = !_isDark;
        }
    }
}
