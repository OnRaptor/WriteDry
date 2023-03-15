using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WriteDry.Db.Models;
using WriteDry.Services;
using WriteDry.Utils;

namespace WriteDry.ViewModels.Component
{
    public class OrderItemViewModel : PropertyChangedBase
    {
        public Order Order { get; set; }
        public List<Orderproduct> OrderProducts { get; set; }
        public float TotalCost { get; set; }
        public float TotalDiscount { get; set; }
        public DateTime ChangeableDate => Order.OrderDate.ToDateTime(TimeOnly.MinValue);
        public bool ShouldDisplayOrderProducts { get; set; }
        public void ToggleDisplayOrderProducts() => ShouldDisplayOrderProducts = !ShouldDisplayOrderProducts;

        private Delegate OnDateChanged;
        public void DateChanged(object sender, SelectionChangedEventArgs args)
        {
            //Execute action from main VM
        }
        public OrderItemViewModel(Order order, Delegate OnDateChanged) {
            this.OnDateChanged = OnDateChanged;
            Order = order;
            OrderProducts = new List<Orderproduct>(order.Orderproducts);
            OrderProducts.ForEach(item => TotalCost += item.ProductArticleNumberNavigation.ProductCost * item.ProductCount);
            OrderProducts.ForEach(item => TotalDiscount += Calculations.GetDiscount(item.ProductArticleNumberNavigation.ProductCost, (float)item.ProductArticleNumberNavigation.ProductDiscountAmount));
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            if (propertyName == nameof(ChangeableDate))
            {

            }
            base.OnPropertyChanged(propertyName);
        }
    }
}
