using Stylet;

namespace WriteDry.ViewModels
{
    public class AdminShellViewModel : Conductor<IScreen>.Collection.OneActive
    {
        public AdminShellViewModel(OrdersViewModel ordersViewModel, ProductsViewModel productsViewModel, ProvidersViewModel providersViewModel, CategoriesViewModel categoriesViewModel)
        {
            this.Items.Add(ordersViewModel);
            this.Items.Add(productsViewModel);
            this.Items.Add(providersViewModel);
            this.Items.Add(categoriesViewModel);
        }
    }
}
