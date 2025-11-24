using Blazored.LocalStorage;
using Gym.Client.Components.Dialog.UserDialog;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Gym.Client.Components.Pages.User_Pages
{
    public class HomePageBase : ComponentBase
    {

        [Inject] protected ILocalStorageService _localStorageService { get; set; } = default!;
        [Inject] protected IDialogService DialogService { get; set; } = default!;
        public bool arrows = true;
        public bool enableSwipeGesture = true;
        public bool autocycle = true;
        public Transition transition = Transition.Slide;
        public bool isMenuOpen = false;

        public void ToggleMenu()
        {
            isMenuOpen = !isMenuOpen;
        }
        public async Task OpenDialogAsync()
        {
            var token = await _localStorageService.GetItemAsStringAsync("authToken");
            var user = await _localStorageService.GetItemAsStringAsync("userRole");
            var options = new DialogOptions() { MaxWidth = MaxWidth.Small, FullWidth = true };

            if (!string.IsNullOrEmpty(user))
            {

                await DialogService.ShowAsync<ApplicationFormDialog>("", options);
            }
            else
            {

                await DialogService.ShowAsync<LoginOrRegisterDialog>("", options);
            }
        }
    }
}
