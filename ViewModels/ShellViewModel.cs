using Stylet;
using System.Collections.Generic;
using WriteDry.Services;

namespace WriteDry.ViewModels
{
    public class ShellViewModel : Conductor<IScreen>.StackNavigation, INavigationControllerDelegate
    {
        public bool BackButtonVisible { get; set; }

        private List<IScreen> screens = new List<IScreen>();
        public void NavigateTo(IScreen screen) {
            screens.Add(screen);
            this.ActivateItem(screen);
            if (screen is AuthViewModel || screens.Count == 1)
                BackButtonVisible = false;
            else
                BackButtonVisible = true;
        }
        public void HandleBackPress() {
            this.GoBack();
            if (ActiveItem is AuthViewModel)
                BackButtonVisible = false;
        }

    }
}
