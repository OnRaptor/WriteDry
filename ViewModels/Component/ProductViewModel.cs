using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using WriteDry.Db.Models;
using WriteDry.Utils;

namespace WriteDry.ViewModels.Component
{
    public class ProductViewModel
    {

        public float? CalculatedCostWithDiscount
            => Calculations.CalculateDiscount(Product.ProductCost, (float)Product.ProductDiscountAmount);

        public Brush DisplayedColor => Product.ProductDiscountAmount > 15 ? new SolidColorBrush(Color.FromRgb(127, 255, 0)) : Brushes.White;

        public bool ShouldDisplayFakeCost => this.CalculatedCostWithDiscount != null;


        public Product Product { get; set; }
        public ProductViewModel(Product product) {
            Product = product;
        }

        public static List<ProductViewModel> CreateCollectionFromProductList(List<Product> products)
            => new List<ProductViewModel>(
                from p in products
                select new ProductViewModel(p)
                );
    }
}
