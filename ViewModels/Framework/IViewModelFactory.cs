namespace WriteDry.ViewModels.Framework {
	public interface IViewModelFactory {
		AuthViewModel CreateAuthViewModel();
		ListViewModel CreateListViewModel();
		OrderViewModel CreateOrderViewModel();
	}
}
