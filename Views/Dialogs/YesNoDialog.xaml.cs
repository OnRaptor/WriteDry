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
    /// Interaction logic for YesNoDialog.xaml
    /// </summary>
    public partial class YesNoDialog : ContentDialog
    {
        public string Text { get; set; } = "";
        public YesNoDialog()
        {
            InitializeComponent();
            this.Loaded += GeneralPromptDialog_Loaded;
        }

        private void GeneralPromptDialog_Loaded(object sender, RoutedEventArgs e)
        {
            this.Content = Text;
        }

    }
}
