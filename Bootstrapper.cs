using Stylet;
using StyletIoC;
using System.Linq;
using System.Windows;
using WriteDry.Services;
using WriteDry.ViewModels;
using WriteDry.ViewModels.Framework;

namespace WriteDry {
	public class Bootstrapper : Bootstrapper<ShellViewModel> {
		private ApplicationContext db = new ApplicationContext();
		protected override void ConfigureIoC(IStyletIoCBuilder builder) {
			// Configure the IoC container in here
			builder.Bind<IViewModelFactory>().ToAbstractFactory();
			builder.Bind<NavigationController>().And<INavigationController>().To<NavigationController>().InSingletonScope();
			builder.Bind<ApplicationContext>().ToInstance(db);
			builder.Bind<ListViewModel>().ToSelf().InSingletonScope();
			var clientService = new ClientService(db);
            builder.Bind<ClientService>().ToInstance(clientService);
			builder.Bind<AdminService>().ToInstance(new AdminService(clientService, db));
		}

		protected override async void OnLaunch() {
			base.OnLaunch();
			var navigationController = this.Container.Get<NavigationController>();
			navigationController.Delegate = this.RootViewModel;
			navigationController.NavigateToAuth();
			this.Container.Get<ListViewModel>();
			await db.EnsureConnectionAsync();
			db.PNames.ToList();
			db.PManufacturers.ToList();
		}

		protected override void OnExit(ExitEventArgs e) {
			db.Dispose();
			base.OnExit(e);
		}
	}
}
