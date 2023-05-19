using ModernWpf.Controls;
using Stylet;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WriteDry.ViewModels.Framework
{
    public class DialogManager : IDisposable
    {
        private readonly IViewManager _viewManager;
        private readonly SemaphoreSlim _dialogLock = new(1, 1);

        public DialogManager(IViewManager viewManager)
        {
            _viewManager = viewManager;
        }

        public async ValueTask<T?> ShowDialogAsync<T>(DialogScreen<T> dialogScreen)
        {
            var view = _viewManager.CreateAndBindViewForModelIfNecessary(dialogScreen);

            await _dialogLock.WaitAsync();
            try
            {
                var dialog = view as ContentDialog;
                dialogScreen.OnLaunch(dialog);
                await dialog.ShowAsync();
                return dialogScreen.DialogResult;
            }
            finally
            {
                _dialogLock.Release();
            }
        }

        /*public string? PromptSaveFilePath(string filter = "All files|*.*", string defaultFilePath = "")
        {
            var dialog = new VistaSaveFileDialog
            {
                Filter = filter,
                AddExtension = true,
                FileName = defaultFilePath,
                DefaultExt = Path.GetExtension(defaultFilePath)
            };

            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }

        public string? PromptDirectoryPath(string defaultDirPath = "")
        {
            var dialog = new VistaFolderBrowserDialog
            {
                SelectedPath = defaultDirPath
            };

            return dialog.ShowDialog() == true ? dialog.SelectedPath : null;
        }*/

        public void Dispose()
        {
            _dialogLock.Dispose();
        }
    }
}
