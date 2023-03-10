using Stylet;
using StyletIoC;
using System.Windows;
using WriteDry.Services;
using WriteDry.ViewModels;
using WriteDry.ViewModels.Framework;

namespace WriteDry
{
    public class Bootstrapper : Bootstrapper<ShellViewModel>
    {
        private ApplicationContext db = new ApplicationContext();
        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            // Configure the IoC container in here
            builder.Bind<IViewModelFactory>().ToAbstractFactory();
            builder.Bind<NavigationController>().And<INavigationController>().To<NavigationController>().InSingletonScope();
            builder.Bind<ApplicationContext>().ToInstance(db);
            builder.Bind<ListViewModel>().ToSelf().InSingletonScope(); ;
            builder.Bind<UserService>().ToSelf().InSingletonScope();
        }

        protected override async void OnLaunch()
        {
            base.OnLaunch();
            var navigationController = this.Container.Get<NavigationController>();
            navigationController.Delegate = this.RootViewModel;
            navigationController.NavigateToAuth();
            this.Container.Get<ListViewModel>();
            await db.EnsureConnectionAsync();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            db.Dispose();
            base.OnExit(e);
        }
    }
}
