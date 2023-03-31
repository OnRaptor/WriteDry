using WriteDry.ViewModels.Component;
using WriteDry.ViewModels.Framework;

namespace WriteDry.ViewModels
{
    public class EditProductDialogResult
    {
        public int Quantity { get; set; }
        public int Discount { get; set; }
        public string Status { get; set; }
        public bool Canceled { get; set; }
    }
    public class EditProductViewModel : DialogScreen<EditProductDialogResult>
    {
        public string Quantity { get; set; }
        public string Discount { get; set; }
        public string Status { get; set; }

        public void Apply()
        {
            this.Close(new()
            {
                Discount = int.Parse(Discount),
                Quantity = int.Parse(Quantity),
                Status = Status
            });
        }

        public void Cancel() => this.Close(new() { Canceled = true });

    }

    public static class EditProductViewModelExtensions
    {
        public static EditProductViewModel CreateEditProductViewModel(this IViewModelFactory viewModelFactory, ProductViewModel product)
        {
            var vm = viewModelFactory.CreateEditProductViewModel();
            vm.Quantity = product.Product.ProductQuantityInStock.ToString();
            vm.Discount = product.Product.ProductDiscountAmount.ToString();
            vm.Status = product.Product.ProductStatus ?? "Активный";
            return vm;
        }
    }

}
