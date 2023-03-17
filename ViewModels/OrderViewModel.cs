using ModernWpf.Controls;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WriteDry.Models;
using WriteDry.PdfMarkup;
using WriteDry.Services;
using WriteDry.Utils;
using Point = WriteDry.Db.Models.Point;

namespace WriteDry.ViewModels
{
    public class OrderViewModel : Screen
    {
        public BindableCollection<Cart.CartItem> CartItems { get; set; }
        public float OrderCost { get; set; }
        public float TotalDiscountAmount { get; set; }
        public string AuthorizedUser { get; set; }

        private ClientService _clientService;
        private ApplicationContext db;
        private List<PointEx> _points;
        private Point _selectedPickupPoint { get; set; }
        private class PointEx
        {
            public string FullAddress { get; set; }
            public Point Point;
        }
        public OrderViewModel(ClientService clientService, ApplicationContext applicationContext) {
            _clientService = clientService;
            db = applicationContext;
        }
        protected override void OnActivate() {
            CartItems = new BindableCollection<Cart.CartItem>(_clientService.UserCart.CartItems);
            CalculateStatistic();
            AuthorizedUser = UserFIO.GetFIO(_clientService.authorizedUser);
            _points = db.Points.Select(point => new PointEx {
                Point = point,
                FullAddress = string.Join(" ", point.City, point.Street, point.House)
            }).ToList();
            base.OnActivate();
        }
        public async void CreateOrder() {
            if (_selectedPickupPoint == null || CartItems.Count == 0) {
                MessageBox.Show("Точка доставки не может быть не выбрана и должно быть выбрано больше 0 товаров");
                return;
            }
            var codeToPickup = new Random().Next(100, 999);
            var orderId = await _clientService.SubmitOrderAsync(_selectedPickupPoint, codeToPickup);
            var generatedMarkup = await Task.Run(() => CheckMarkup.GenerateMarkup(
                _clientService.UserCart,
                OrderCost,
                TotalDiscountAmount,
                _selectedPickupPoint,
                codeToPickup,
                orderId
                ));

            WindowsShell.OpenFileInExplorer(generatedMarkup, true);
            CartItems.Clear();
            _clientService.UserCart.CartItems.Clear();
            CalculateStatistic();
        }
        public void RemoveItem(Cart.CartItem item) {
            CartItems.Remove(item);
            _clientService.UserCart.CartItems.Remove(item);
            CalculateStatistic();
        }
        public void OnPickupPointFocus(object sender, RoutedEventArgs args)
            => ((AutoSuggestBox)sender).ItemsSource = _points;
        public void OnPickupPointSuggestionChanged(object sender, AutoSuggestBoxSuggestionChosenEventArgs args) =>
            _selectedPickupPoint = ((PointEx)args.SelectedItem).Point;

        public void OnPickupPointFinding(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args) {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput) {
                sender.ItemsSource = _points.FindAll(item => item.FullAddress.ToLower().Contains(sender.Text.ToLower()));
                _selectedPickupPoint = null;
            }
        }

        public void CalculateStatistic() {
            if (CartItems == null || CartItems.Count == 0) {
                OrderCost = 0;
                TotalDiscountAmount = 0;
                return;
            }
            float cost = 0;
            float discount = 0;
            foreach (var item in CartItems) {
                cost += Calculations.CalculateDiscount(item.Product.ProductCost * item.Count, (float)item.Product.ProductDiscountAmount);
                discount += Calculations.GetDiscount(item.Product.ProductCost * item.Count, (float)item.Product.ProductDiscountAmount);
            }
            OrderCost = cost;
            TotalDiscountAmount = discount;
        }
    }
}
