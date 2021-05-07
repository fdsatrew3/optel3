using OPTEL.UI.Desktop.ViewModels;
using System.Windows;

namespace OPTEL.UI.Desktop.Views
{
    /// <summary>
    /// Логика взаимодействия для ExtruderCalibrationsWindow.xaml
    /// </summary>
    public partial class ExtruderCalibrationsWindow : Window
    {
        public ExtruderCalibrationsWindow()
        {
            InitializeComponent();
            this.DataContext = new ExtruderCalibrationsViewModel();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            return;
        }
    }
}
