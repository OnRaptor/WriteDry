using Stylet;
using WriteDry.ViewModels.Framework;

namespace WriteDry.Services {
	public interface INavigationController {
		void NavigateToAuth();
		void NavigateToProducts();
		void NavigateToOrders();
		void NavigateToAdminShell();
	}

	public interface INavigationControllerDelegate {
		void NavigateTo(IScreen screen);
	}

	public class NavigationController : INavigationController {
		public INavigationControllerDelegate Delegate { get; set; }

		private IViewModelFactory factory;

		public NavigationController(IViewModelFactory factory) {
			this.factory = factory;
		}

		public void NavigateToAuth() => this.Delegate?.NavigateTo(factory.CreateAuthViewModel());
		public void NavigateToProducts() => this.Delegate?.NavigateTo(factory.CreateListViewModel());
		public void NavigateToOrders() => this.Delegate?.NavigateTo(factory.CreateOrderViewModel());
		public void NavigateToAdminShell() => this.Delegate?.NavigateTo(factory.CreateAdminShellViewModel());

	}
}
