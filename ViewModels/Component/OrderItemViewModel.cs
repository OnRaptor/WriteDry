using Stylet;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using WriteDry.Db.Models;
using WriteDry.Utils;
using WriteDry.ViewModels.Framework;

namespace WriteDry.ViewModels.Component
{
    public class OrderStatusItem : PropertyChangedBase
    {
        public bool IsNew { get; set; }
        public string Title { get; set; }

        public static OrderStatusItem CreateStatusFromString(string name)
            => new OrderStatusItem { IsNew = name == "Новый", Title = name };
    }

    public class OrderItemViewModel : PropertyChangedBase
    {
        public Order Order { get; set; }
        public List<Orderproduct> OrderProducts { get; set; }
        public float TotalCost { get; set; }
        public float TotalDiscount { get; set; }
        public DateTime DeliveryDate => Order.OrderDate.ToDateTime(TimeOnly.MinValue);
        public bool ShouldDisplayOrderProducts { get; set; }
        public int CurrentStatus { get; set; }
        public void ToggleDisplayOrderProducts() => ShouldDisplayOrderProducts = !ShouldDisplayOrderProducts;

        public OnDateChanged onDateChanged;
        public OnStatusChange onStatusChange;

        public delegate void OnDateChanged(OrderItemViewModel item, DateTime newDate);
        public delegate void OnStatusChange(OrderItemViewModel item, OrderStatusItem newStatus);
        public void DateChanged(object sender, SelectionChangedEventArgs args) {
            if ((sender as Control).IsLoaded)
                onDateChanged?.Invoke(this, (DateTime)args.AddedItems[0]); //Execute delegate from main VM
        }

        public void StatusChanged(object sender, SelectionChangedEventArgs args) {
            
            if ((sender as Control).IsLoaded)
                onStatusChange?.Invoke(this, (OrderStatusItem)args.AddedItems[0]); //Execute delegate from main VM
        }
        public void Init() {
            CurrentStatus = OrderStatusItem.CreateStatusFromString(Order.OrderStatus).IsNew ? 0 : 1;
            OrderProducts = new List<Orderproduct>(Order.Orderproducts);
            OrderProducts.ForEach(item => TotalCost += item.ProductArticleNumberNavigation.ProductCost * item.ProductCount);
            OrderProducts.ForEach(item => TotalDiscount += Calculations.GetDiscount(item.ProductArticleNumberNavigation.ProductCost, (float)item.ProductArticleNumberNavigation.ProductDiscountAmount));
        }
    }

    public static class OrderItemVMExtensions
    {
        public static OrderItemViewModel CreateOrderItemViewModel(
            this IViewModelFactory factory,
            Order order,
            OrderItemViewModel.OnDateChanged OnDateChanged,
            OrderItemViewModel.OnStatusChange OnStatusChange) {
            var vm = factory.CreateOrderItemViewModel();
            vm.onDateChanged = OnDateChanged;
            vm.onStatusChange = OnStatusChange;
            vm.Order = order;
            vm.Init();
            return vm;
        }
    }
}
