using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using WriteDry.Db.Models;
using WriteDry.Services;
using WriteDry.Utils;
using WriteDry.ViewModels.Component;
using WriteDry.ViewModels.Framework;

namespace WriteDry.ViewModels
{
    public class OrdersViewModel : Screen
    {
        public string UserName { get; set; }
        public int MaxOrdersCount { get; set; }
        public SortItem SelectedSort { get; set; } = new SortItem { SortProduct = SortProduct.Desc };
        public FilterItem SelectedFilter { get; set; } = new FilterItem { FilterProduct = FilterProduct.All };
        public BindableCollection<OrderItemViewModel> Orders { get; set; }

        private AdminService adminService;
        private IViewModelFactory vmFactory;
        private ApplicationContext dbContext;
        private List<Order> _ordersCache; //data from db
        private List<OrderItemViewModel> _orderItemsCache; //converted data from db for search work
        public OrdersViewModel(AdminService adminService, IViewModelFactory viewModelFactory, ApplicationContext applicationContext) {
            this.DisplayName = "Заказы";
            this.adminService = adminService;
            this.vmFactory = viewModelFactory;
            this.dbContext = applicationContext;
        }

        protected override void OnPropertyChanged(string propertyName) {
            if (propertyName == nameof(SelectedFilter)) {
                ApplyFilter();
                ApplySort();
            }
            else if (propertyName == nameof(SelectedSort))
                ApplySort();
            base.OnPropertyChanged(propertyName);
        }

        public void ApplySort() {
            if (SelectedSort.SortProduct == SortProduct.Desc)
                Orders = new BindableCollection<OrderItemViewModel>(Orders.OrderByDescending(item => item.TotalCost));
            else  // Ascending
                Orders = new BindableCollection<OrderItemViewModel>(Orders.OrderBy(item => item.TotalCost));
        }

        public void ApplyFilter() {
            switch (SelectedFilter.FilterProduct) {
                case FilterProduct.All:
                    LoadOrdersToViewItems();
                    break;
                case FilterProduct.Low:
                    Orders = new BindableCollection<OrderItemViewModel>(_orderItemsCache.Where(
                        item => item.TotalDiscount < 10));
                    break;
                case FilterProduct.Medium:
                    Orders = new BindableCollection<OrderItemViewModel>(_orderItemsCache.Where(
                        item => item.TotalDiscount >= 10 && item.TotalDiscount < 15));
                    break;
                case FilterProduct.High:
                    Orders = new BindableCollection<OrderItemViewModel>(_orderItemsCache.Where(
                        item => item.TotalDiscount >= 15));
                    break;
            }
        }

        public void OnDateChange(OrderItemViewModel item, DateTime newDate) {
            var order = dbContext.Orders.Find(item.Order.OrderId);
            if (order == null || order.OrderDeliveryDate == DateOnly.FromDateTime(newDate))//Ignore same changes
                return;
            order.OrderDeliveryDate = DateOnly.FromDateTime(newDate);
            dbContext.SaveChanges();
        }
        public void OnStatusChange(OrderItemViewModel item, OrderStatusItem newStatus) {
            var order = dbContext.Orders.Find(item.Order.OrderId);
            if (order == null || order.OrderStatus == newStatus.Title)//Ignore same changes
                return;
            order.OrderStatus = newStatus.Title;
            dbContext.SaveChanges();
        }
        private void LoadOrdersToViewItems() => Orders = new BindableCollection<OrderItemViewModel>(_orderItemsCache);
        private void LoadOrdersToViewItems(List<OrderItemViewModel> items) => Orders = new BindableCollection<OrderItemViewModel>(items);
        protected override void OnViewLoaded() {
            UserName = UserFIO.GetFIO(adminService.AuthorizedUser);
            _ordersCache = adminService.Orders;
            MaxOrdersCount = _ordersCache.Count;
            Orders = new BindableCollection<OrderItemViewModel>(_ordersCache.Select(
                item => vmFactory.CreateOrderItemViewModel(
                    item,
                    OnDateChange,
                    OnStatusChange
                    )
            ));
            _orderItemsCache = Orders.ToList();
        }
    }
}
