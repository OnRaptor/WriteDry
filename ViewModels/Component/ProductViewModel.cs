using Stylet;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
 
using WriteDry.Utils;

namespace WriteDry.ViewModels.Component
{
    public class ProductViewModel : PropertyChangedBase
    {

        public float? CalculatedCostWithDiscount
            => ProductCalculations.CalculateCostWithDiscount(Product.ProductCost, (float)Product.ProductDiscountAmount);

        public Brush DisplayedColor => Product.ProductDiscountAmount > 15 ? new SolidColorBrush(Color.FromRgb(127, 255, 0)) : Brushes.White;

        public bool ShouldDisplayFakeCost => this.CalculatedCostWithDiscount != null;

        public Product Product { get; set; }
        public ProductViewModel(Product product)
        {
            Product = product;
        }

        public static List<ProductViewModel> CreateCollectionFromProductList(List<Product> products)
            => new List<ProductViewModel>(
                from p in products
                select new ProductViewModel(p)
                );
    }
    public static class ProductsExtensions
    {
        public static List<ProductViewModel> Search(this BindableCollection<ProductViewModel> items, string text)
        {
            return (
                from p in items
                where p.Product.ProductNameNavigation.ProductName.ToLower().Contains(text.ToLower())
                select p
                ).ToList();
        }
    }
}
