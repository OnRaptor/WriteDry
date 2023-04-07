using Stylet;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using WriteDry.Services;
using WriteDry.ViewModels.Component;
using WriteDry.ViewModels.Framework;

namespace WriteDry.ViewModels
{
    public class AuthViewModel : Screen
    {

        public string Login { get; set; }
        public string Password { get; set; }

        private NavigationController _navigation;
        private ClientService _clientService;
        private DialogManager _dialogManager;
        private IViewModelFactory _viewModelFactory;

        public AuthViewModel(NavigationController navigation, ClientService clientService, DialogManager dialogManager, IViewModelFactory viewModelFactory = null)
        {
            _navigation = navigation;
            _clientService = clientService;
            _clientService.OnAuthStateChanged += HandleAuthEvents;
            _dialogManager = dialogManager;
            _viewModelFactory = viewModelFactory;
        }
        public void OnPasswordChange(object sender, EventArgs e)
        {
            var passwordBox = (PasswordBox)sender;
            Password = passwordBox.Password;
        }

        public void EnterAsGuest() => _clientService.LoginAsGuest();
        public async void Auth() => await _clientService.Login(Login, Password);
        public void GoToReigster() => _navigation.NavigateToRegistration();

        private async void HandleAuthEvents(object sender, ClientService.AuthArgs e)
        {
            if (e.Failed)
                await _dialogManager.ShowDialogAsync(_viewModelFactory.CreateMessageBoxViewModel("Ошибка", "Проверьте данные"));
            else if (e.IsAdmin)
                _navigation.NavigateToAdminShell();
            else if (e.isGuest || e.newUserAuth != null)
                _navigation.NavigateToProducts();
        }
/*
#if DEBUG
        protected override async void OnViewLoaded()
        {
            await Task.Delay(400);
            Login = "loginDEpxl2018";
            Password = "P6h4Jq";
            Auth();
            base.OnViewLoaded();
        }
#endif*/
    }
}
