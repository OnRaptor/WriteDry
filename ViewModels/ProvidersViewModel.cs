using ModernWpf.Controls;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WriteDry.Services;
using WriteDry.ViewModels.Component;
using WriteDry.ViewModels.Framework;
using WriteDry.Views.Dialogs;

namespace WriteDry.ViewModels
{
    public class ProvidersViewModel : Screen
    {
        public BindableCollection<Provider> Providers { get; set; }

        private NavigationController navigationController;
        private ApplicationContext dbContext;
        private AdminService _adminService;
        private DialogManager _dialogManager;
        private IViewModelFactory _viewModelFactory;

        public ProvidersViewModel(NavigationController navigationController, ApplicationContext dbContext, AdminService adminService, DialogManager dialogManager, IViewModelFactory viewModelFactory)
        {
            this.DisplayName = "Поставщики";
            this.navigationController = navigationController;
            this.dbContext = dbContext;
            _adminService = adminService;
            _dialogManager = dialogManager;
            _viewModelFactory = viewModelFactory;
        }

        public async void EditProvider(Provider item)
        {
            var dialog = new GeneralPromptDialog { Title="Введите новое название для поставщика", Placeholder=item.ProviderName };
            if (await dialog.ShowAsync() == ContentDialogResult.Primary &&
                !string.IsNullOrWhiteSpace(dialog.Text) &&
                dialog.Text != item.ProviderName
                )
            {
                dbContext.Providers.Find(item.Id).ProviderName = dialog.Text;
                dbContext.SaveChanges();
                LoadProviders();
            }
        }

        public async void DeleteProvider(Provider item)
        {
            var dialog = new YesNoDialog { Text = "Вы действительно хотите удалить поставщика?" };
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                try
                {
                    dbContext.Providers.Remove(item);
                    await dbContext.SaveChangesAsync();
                    LoadProviders();
                }
                catch
                {
                    await _dialogManager.ShowDialogAsync(
                        _viewModelFactory.CreateMessageBoxViewModel("Ошибка", "Нельзя удалить, так как поставщик указан в существующем заказе")
                        );
                }
            }
        }
        public async void AddProvider()
        {
            var dialog = new GeneralPromptDialog { Title = "Введите название для поставщика" };
            if (await dialog.ShowAsync() == ContentDialogResult.Primary &&
                !string.IsNullOrWhiteSpace(dialog.Text)
                )
            {
                dbContext.Providers.Add(new Provider { ProviderName = dialog.Text }); 
                dbContext.SaveChanges();
                LoadProviders();
            }
        }
        private void LoadProviders()
        {
            Providers = new BindableCollection<Provider>(dbContext.Providers.ToList());
        }
        protected override void OnActivate()
        {
            LoadProviders();
        }
    }
}
