using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WriteDry.Db.Models;
using WriteDry.Services;
using WriteDry.Utils;
using WriteDry.ViewModels.Component;

namespace WriteDry.ViewModels
{
    public class OrdersViewModel : Screen
    {
        public string UserName { get; set; }
        public int MaxOrdersCount { get; set; }
        public string SearchText { get; set; }
        public SortItem SelectedSort { get; set; } = new SortItem { SortProduct = SortProduct.Desc };
        public FilterItem SelectedFilter { get; set; } = new FilterItem { FilterProduct = FilterProduct.All };
        public BindableCollection<OrderItemViewModel> Orders { get; set; }

        private AdminService adminService;
        private List<Order> orderSnapshot;
        public OrdersViewModel(AdminService adminService) {
            this.DisplayName = "Заказы";
            this.adminService = adminService;
        }
        protected override void OnViewLoaded()
        {
            UserName = UserFIO.GetFIO(adminService.AuthorizedUser);
            orderSnapshot = adminService.Orders;
            MaxOrdersCount = orderSnapshot.Count;
            Orders = new BindableCollection<OrderItemViewModel>(orderSnapshot.Select(item => new OrderItemViewModel(item)));
        }
    }
}
