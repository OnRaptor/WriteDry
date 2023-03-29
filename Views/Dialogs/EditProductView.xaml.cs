using ModernWpf.Controls;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace WriteDry.Views
{
    /// <summary>
    /// Логика взаимодействия для EditProductView.xaml
    /// </summary>
    public partial class EditProductView : ContentDialog
    {
        private static readonly Regex _regex = new Regex("^\\d+$");
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
        public EditProductView()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (IsTextAllowed(e.Text))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }
    }
}
