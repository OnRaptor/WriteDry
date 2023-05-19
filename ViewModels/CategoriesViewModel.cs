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
    public class CategoriesViewModel : Screen
    {
        public BindableCollection<Pcategory> Categories { get; set; }

        private NavigationController navigationController;
        private ApplicationContext dbContext;
        private AdminService _adminService;
        private DialogManager _dialogManager;
        private IViewModelFactory _viewModelFactory;

        public CategoriesViewModel(NavigationController navigationController, ApplicationContext dbContext, AdminService adminService, DialogManager dialogManager, IViewModelFactory viewModelFactory)
        {
            this.DisplayName = "Категории";
            this.navigationController = navigationController;
            this.dbContext = dbContext;
            _adminService = adminService;
            _dialogManager = dialogManager;
            _viewModelFactory = viewModelFactory;
        }

        public async void EditCategory(Pcategory item)
        {
            var dialog = new GeneralPromptDialog { Title = "Введите новое название для категории", Placeholder = item.CategoryName };
            if (await dialog.ShowAsync() == ContentDialogResult.Primary &&
                !string.IsNullOrWhiteSpace(dialog.Text) &&
                dialog.Text != item.CategoryName
                )
            {
                dbContext.Pcategories.Find(item.Id).CategoryName = dialog.Text;
                dbContext.SaveChanges();
                LoadCategories();
            }
        }

        public async void DeleteCategory(Pcategory item)
        {
            var dialog = new YesNoDialog { Text = "Вы действительно хотите удалить категорию?" };
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                try
                {
                    dbContext.Pcategories.Remove(item);
                    await dbContext.SaveChangesAsync();
                    LoadCategories();
                }
                catch
                {
                    await _dialogManager.ShowDialogAsync(
                        _viewModelFactory.CreateMessageBoxViewModel("Ошибка", "Нельзя удалить, так как категория используется в существующем заказе")
                        );
                }
            }
        }

        public async void AddCategory()
        {
            var dialog = new GeneralPromptDialog { Title = "Введите название для категории" };
            if (await dialog.ShowAsync() == ContentDialogResult.Primary &&
                !string.IsNullOrWhiteSpace(dialog.Text)
                )
            {
                dbContext.Pcategories.Add(new Pcategory { CategoryName = dialog.Text });
                dbContext.SaveChanges();
                LoadCategories();
            }
        }
        private void LoadCategories()
        {
            Categories = new BindableCollection<Pcategory>(dbContext.Pcategories.ToList());
        }
        protected override void OnActivate()
        {
            LoadCategories();
        }
    }
}
