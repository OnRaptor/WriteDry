using WriteDry.ViewModels.Framework;

namespace WriteDry.ViewModels.Component
{
    public class MessageBoxViewModel : DialogScreen<bool>
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string PrimaryButtonText { get; set; } = "OK";

        public void CloseDialog()
        {
            this.Close(true);
        }
    }

    public static class MessageBoxViewModelExtensions
    {
        public static MessageBoxViewModel CreateMessageBoxViewModel(this IViewModelFactory factory, string title, string message)
        {
            var vm = factory.CreateMessageBoxViewModel();
            vm.Title = title;
            vm.Message = message;
            return vm;
        }

        public static MessageBoxViewModel CreateMessageBoxViewModel(this IViewModelFactory factory, string title, string message, string primaryButtonText)
        {
            var vm = factory.CreateMessageBoxViewModel();
            vm.Title = title;
            vm.Message = message;
            vm.PrimaryButtonText = primaryButtonText;
            return vm;
        }
    }
}
