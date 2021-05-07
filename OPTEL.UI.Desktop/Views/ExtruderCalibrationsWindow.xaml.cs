using OPTEL.UI.Desktop.ViewModels;
using OPTEL.UI.Desktop.ViewModels.Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
            this.MouseRightButtonUp += new MouseButtonEventHandler(Window1_MouseRightButtonUp);
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
        void Window1_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            while ((dep != null) && !(dep is DataGridCell))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }
            if (dep == null)
            {
                ElementsList.SelectedItem = null;
            }

            if (dep is DataGridCell)
            {
                DataGridCell cell = dep as DataGridCell;
                cell.Focus();

                while ((dep != null) && !(dep is DataGridRow))
                {
                    dep = VisualTreeHelper.GetParent(dep);
                }
                DataGridRow row = dep as DataGridRow;
                ElementsList.SelectedItem = row.DataContext;
            }
        }
    }
}
