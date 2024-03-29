﻿using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;

using WriteDry.Services;
using WriteDry.Utils;
using WriteDry.ViewModels.Component;
using WriteDry.ViewModels.Framework;

namespace WriteDry.ViewModels
{
    public class TypeItem
    {
        public string Title { get; set; }

        public ItemTypeProduct Type { get; set; }
    }
    public enum ItemTypeProduct
    {
        New, Finished
    }
    public class OrdersViewModel : Screen
    {
        public string UserName { get; set; }
        public string SearchText { get; set; }
        public int MaxOrdersCount { get; set; }
        public SortItem SelectedSort { get; set; } = new SortItem { SortProduct = SortProduct.Desc };
        public FilterItem SelectedFilter { get; set; } = new FilterItem { FilterProduct = FilterProduct.All };
        public TypeItem SelectedTypeProduct { get; set; } = new TypeItem { Type = ItemTypeProduct.New, Title="Только новые" };
        public BindableCollection<OrderItemViewModel> Orders { get; set; }

        private AdminService adminService;
        private IViewModelFactory vmFactory;
        private ApplicationContext dbContext;
        private List<Order> _ordersCache; //data from db
        private List<OrderItemViewModel> _orderItemsCache; //converted data from db for search work
        public OrdersViewModel(AdminService adminService, IViewModelFactory viewModelFactory, ApplicationContext applicationContext)
        {
            this.DisplayName = "Заказы";
            this.adminService = adminService;
            this.vmFactory = viewModelFactory;
            this.dbContext = applicationContext;
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            if (propertyName == nameof(SelectedFilter))
            {
                ApplyFilter();
                ApplyProductType();
                ApplySort();
            }
            else if (propertyName == nameof(SelectedSort))
            {
                ApplyProductType();
                ApplySort();
            }
            else if (propertyName == nameof(SelectedTypeProduct))
            {
                ApplyFilter();
                ApplyProductType();
                ApplySort();
                CalculateMaxOrdersCount();
            }

            base.OnPropertyChanged(propertyName);
        }
        public void CalculateMaxOrdersCount()
        {
            MaxOrdersCount =
                SelectedTypeProduct.Type == ItemTypeProduct.New ?
                _ordersCache.Where(item => item.OrderStatus == "Новый").Count() :
                _ordersCache.Where(item => item.OrderStatus == "Списан").Count();
        }
        public void ApplyProductType()
        {
            if (Orders != null)
                Orders = new BindableCollection<OrderItemViewModel>(Orders.Where(item => item.CurrentStatus == (SelectedTypeProduct.Type == ItemTypeProduct.New ? 0 : 1)));
            else
            {
                LoadOrdersToViewItems();
                Orders = new BindableCollection<OrderItemViewModel>(Orders.Where(item => item.CurrentStatus == (SelectedTypeProduct.Type == ItemTypeProduct.New ? 0 : 1)));
            }
        }
        public void ApplySearch()
        {
            OrderItemViewModel? item = null;
            try
            {
                item = _orderItemsCache.Find(item => item.Order.OrderId == int.Parse(SearchText));
            }
            catch
            {
                Orders = new BindableCollection<OrderItemViewModel>(_orderItemsCache);
            }
            if (item != null)
                Orders = new BindableCollection<OrderItemViewModel> { item };
        }

        public void ApplySort()
        {
            if (SelectedSort.SortProduct == SortProduct.Desc)
                Orders = new BindableCollection<OrderItemViewModel>(Orders.OrderByDescending(item => item.TotalCost));
            else  // Ascending
                Orders = new BindableCollection<OrderItemViewModel>(Orders.OrderBy(item => item.TotalCost));
        }

        public void ApplyFilter()
        {
            switch (SelectedFilter.FilterProduct)
            {
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

        public void OnDateChange(OrderItemViewModel item, DateTime newDate)
        {
            var order = dbContext.Orders.Find(item.Order.OrderId);
            if (order == null || order.OrderDeliveryDate == newDate)//Ignore same changes
                return;
            order.OrderDeliveryDate = newDate;
            dbContext.SaveChanges();
        }
        public void OnStatusChange(OrderItemViewModel item, OrderStatusItem newStatus)
        {
            var order = dbContext.Orders.Find(item.Order.OrderId);
            if (order == null || order.OrderStatus == newStatus.Title)//Ignore same changes
                return;
            order.OrderStatus = newStatus.Title;
            dbContext.SaveChanges();
            adminService.LoadOrders();
            OnViewLoaded();
        }
        private void LoadOrdersToViewItems() => Orders = new BindableCollection<OrderItemViewModel>(_orderItemsCache);
        private void LoadOrdersToViewItems(List<OrderItemViewModel> items) => Orders = new BindableCollection<OrderItemViewModel>(items);
        protected override void OnViewLoaded()
        {
            UserName = UserFIO.GetFIO(adminService.AuthorizedUser);
            _ordersCache = adminService.Orders;
            CalculateMaxOrdersCount();
            Orders = new BindableCollection<OrderItemViewModel>(_ordersCache.Select(
                item => vmFactory.CreateOrderItemViewModel(
                    item,
                    OnDateChange,
                    OnStatusChange,
                    dbContext.Orderproducts.ToList().FindAll(p => p.OrderId == item.OrderId)
                    )
            ));
            _orderItemsCache = Orders.ToList();
            ApplyFilter();
            ApplyProductType();
            ApplySort();
        }
    }
}
