using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace OPTEL.UI.Desktop.Views.Core
{
    public class DatabaseEntityView : Window
    {
        public DatabaseEntityView()
        {
            MouseRightButtonUp += new MouseButtonEventHandler(OnWindowRightButtonUp);
        }

        private void OnWindowRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            while ((dep != null) && !(dep is DataGridCell))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }
            if (dep == null)
            {
                SetSelectedItem(null);
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
                SetSelectedItem(row.DataContext);
            }
        }
        public virtual void SetSelectedItem(object item) { }
    }
}
