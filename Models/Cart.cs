using System.Collections.Generic;
using WriteDry.Db.Models;
using WriteDry.ViewModels.Component;

namespace WriteDry.Models
{
    public class Cart
    {
        public class CartItem : ProductViewModel
        {
            public CartItem(Product product) : base(product) { }
            public int Count { get; set; } = 1;
        }
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
