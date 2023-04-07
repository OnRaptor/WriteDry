using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WriteDry.Views.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для GeneralPromptDialog.xaml
    /// </summary>
    public partial class GeneralPromptDialog : ContentDialog
    {
        public string Placeholder { get; set; } = "";
        public string Text { get; set; } = "";

        public GeneralPromptDialog()
        {
            InitializeComponent();
            this.Loaded += GeneralPromptDialog_Loaded;
        }

        private void GeneralPromptDialog_Loaded(object sender, RoutedEventArgs e)
        {
            text_box.Text = Placeholder;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Text = text_box.Text;
        }
    }
}
