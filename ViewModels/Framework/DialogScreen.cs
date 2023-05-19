using ModernWpf.Controls;
using Stylet;
using System;

namespace WriteDry.ViewModels.Framework
{
    public abstract class DialogScreen<T> : PropertyChangedBase
    {
        public T? DialogResult { get; private set; }

        public event EventHandler? Closed;

        public ContentDialog root;

        public virtual void OnLaunch(ContentDialog contentDialog) { 
            this.root = contentDialog;
        }


        public void Close(T? dialogResult = default)
        {
            DialogResult = dialogResult;
            Closed?.Invoke(this, EventArgs.Empty);
            root.Hide();
        }
    }

    public abstract class DialogScreen : DialogScreen<bool?>
    {
    }
}
