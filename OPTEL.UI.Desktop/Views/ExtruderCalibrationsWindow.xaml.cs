using OPTEL.UI.Desktop.ViewModels;
using OPTEL.UI.Desktop.ViewModels.Core;
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
            Window w = sender as Window;
            if (w != null)
            {
                DatabaseEntityViewModel vm = (w.DataContext as DatabaseEntityViewModel);
                vm.CheckForUnsavedChangesOnWindowClosing.Execute(null);
                e.Cancel = !vm.IsCloseAllowed;
            }
        }
    }
}
