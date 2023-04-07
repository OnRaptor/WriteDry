using FluentValidation;
using Stylet;
using WriteDry.Services;
using WriteDry.ViewModels.Component;
using WriteDry.ViewModels.Framework;

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
        private DialogManager _dialogManager;
        private IViewModelFactory _viewModelFactory;
        public RegisterViewModel(ClientService clientService, NavigationController navigationController, IModelValidator<RegisterViewModel> validator, DialogManager dialogManager, IViewModelFactory viewModelFactory) : base(validator)
        {
            _clientService = clientService;
            _clientService.OnAuthStateChanged += _clientService_OnAuthStateChanged;
            _navigationController = navigationController;
            this.PropertyChanged += OnPropertyChanged;
            _dialogManager = dialogManager;
            _viewModelFactory = viewModelFactory;
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
        private async void _clientService_OnAuthStateChanged(object sender, ClientService.AuthArgs e)
        {
            if (e.IsSuccesfulRegistration)
                _navigationController.NavigateToProducts();
            else if(!e.IsSuccesfulRegistration || e.newUserAuth != null)
                await _dialogManager.ShowDialogAsync(_viewModelFactory.CreateMessageBoxViewModel("Ошибка", "Пользователь уже существует"));
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
