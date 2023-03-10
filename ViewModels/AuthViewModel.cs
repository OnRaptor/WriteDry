using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WriteDry.Services;

namespace WriteDry.ViewModels
{
    public class AuthViewModel : Screen
    {

        public string Login { get; set; }
        public string Password{ get; set; }

        private NavigationController _navigation;
        private UserService _userService;

        public AuthViewModel(NavigationController navigation, UserService userService)
        {
            _navigation = navigation;
            _userService = userService;
            _userService.onAuthChanged += HandleAuthEvents;
        }
        public void OnPasswordChange(object sender, EventArgs e)
        {
            var passwordBox = (PasswordBox)sender;
            Password = passwordBox.Password;
        }

        public void EnterAsGuest() => _userService.LoginAsGuest();
        public async void Auth() => await _userService.Login(Login, Password);

        private void HandleAuthEvents(object sender, UserService.AuthArgs e)
        {
            if (e.Failed)
                MessageBox.Show("Проверьте данные");
            else
                _navigation.NavigateToProducts();
        }
    }
}
