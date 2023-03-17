using Stylet;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WriteDry.Services;

namespace WriteDry.ViewModels
{
    public class AuthViewModel : Screen
    {

        public string Login { get; set; }
        public string Password { get; set; }

        private NavigationController _navigation;
        private ClientService _clientService;

        public AuthViewModel(NavigationController navigation, ClientService clientService) {
            _navigation = navigation;
            _clientService = clientService;
            _clientService.OnAuthStateChanged += HandleAuthEvents;
        }
        public void OnPasswordChange(object sender, EventArgs e) {
            var passwordBox = (PasswordBox)sender;
            Password = passwordBox.Password;
        }

        public void EnterAsGuest() => _clientService.LoginAsGuest();
        public async void Auth() => await _clientService.Login(Login, Password);

        private void HandleAuthEvents(object sender, ClientService.AuthArgs e) {
            if (e.Failed)
                MessageBox.Show("Проверьте данные");
            else if (e.IsAdmin)
                _navigation.NavigateToAdminShell();
            else
                _navigation.NavigateToProducts();
        }

#if DEBUG
        protected override async void OnViewLoaded() {
            await Task.Delay(400);
            Login = "loginDEpxl2018";
            Password = "P6h4Jq";
            Auth();
            base.OnViewLoaded();
        }
#endif
    }
}
