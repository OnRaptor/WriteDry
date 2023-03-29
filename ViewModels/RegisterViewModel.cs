using FluentValidation;
using Stylet;
using WriteDry.Services;

namespace WriteDry.ViewModels
{
    public class RegisterViewModel : Screen
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string UserPatronymic { get; set; }
        public bool CanProceedRegister { get; set; }

        private ClientService _clientService;
        private NavigationController _navigationController;
        public RegisterViewModel(ClientService clientService, NavigationController navigationController, IModelValidator<RegisterViewModel> validator) : base(validator)
        {
            _clientService = clientService;
            _clientService.OnAuthStateChanged += _clientService_OnAuthStateChanged;
            _navigationController = navigationController;
            this.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (
                e.PropertyName == nameof(Login) ||
                e.PropertyName == nameof(Password) ||
                e.PropertyName == nameof(UserName) ||
                e.PropertyName == nameof(UserSurname) ||
                e.PropertyName == nameof(UserPatronymic)
                )
                CanProceedRegister = this.Validate();
        }
        protected override void OnViewLoaded()
        {
            this.Validate();
        }
        private void _clientService_OnAuthStateChanged(object sender, ClientService.AuthArgs e)
        {
            if (e.IsSuccesfulRegistration)
                _navigationController.NavigateToProducts();
        }
        public async void Register()
        {
            if (CanProceedRegister)
                await _clientService.RegisterUser(
                    UserName,
                    UserSurname,
                    UserPatronymic,
                    Login,
                    Password
                    );
        }
    }

    public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
    {
        public RegisterViewModelValidator()
        {
            RuleFor(e => e.UserName).NotEmpty().WithMessage("Поле не может быть пустым");
            RuleFor(e => e.UserPatronymic).NotEmpty().WithMessage("Поле не может быть пустым");
            RuleFor(e => e.UserSurname).NotEmpty().WithMessage("Поле не может быть пустым");
            RuleFor(e => e.Login).NotEmpty().WithMessage("Поле не может быть пустым");
            RuleFor(e => e.Password).NotEmpty().WithMessage("Поле не может быть пустым");
        }
    }
}
