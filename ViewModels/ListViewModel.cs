using Stylet;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WriteDry.Db.Models;
using WriteDry.Services;

namespace WriteDry.ViewModels
{
    public class ListViewModel : Screen
    {
        public bool CanCreateOrder { get; set; }
        public string UserName { get; set; }
        public int MaxProductsCount { get; set; }
        public string SearchText { get; set; }
        public BindableCollection<Product> Products { get; set; } = new BindableCollection<Product>();

        private NavigationController navigationController;
        private UserService userService;
        private ApplicationContext dbContext;
        public ListViewModel(NavigationController _navController, UserService userService, ApplicationContext dbContext)
        {
            navigationController = _navController;
            this.userService = userService;
            this.dbContext = dbContext;
            userService.onAuthChanged += HandleAuthState;
        }

        private void HandleAuthState(object sender, UserService.AuthArgs e)
        {
            UserName = e.isGuest ? "Гость" : e.newUserAuth.UserName;
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            if (propertyName == nameof(SearchText))
            {
                Products.Clear();
                Products.AddRange((from p in Products
                           where p.ProductDescription.Contains(SearchText)
                           select p).ToList());
            }
            base.OnPropertyChanged(propertyName);
        }
        public void GoToOrders () => navigationController.NavigateToOrders();

        protected override void OnActivate()
        {
            Products.AddRange(dbContext.Products.ToList());
            MaxProductsCount = Products.Count;
            base.OnActivate();
        }
    }
}
