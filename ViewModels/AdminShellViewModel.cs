using Stylet;

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
