using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WriteDry.ViewModels
{
    public class AdminShellViewModel : Conductor<IScreen>.Collection.OneActive
    {
        public AdminShellViewModel(OrdersViewModel ordersViewModel, ProductsViewModel productsViewModel) {
            this.Items.Add(ordersViewModel);
            this.Items.Add(productsViewModel);
        }
    }
}
