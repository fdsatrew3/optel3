using OPTEL.UI.Desktop.Models;
using OPTEL.UI.Desktop.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;

namespace OPTEL.UI.Desktop.Views
{
    /// <summary>
    /// Логика взаимодействия для ErrorsListWindow.xaml
    /// </summary>
    public partial class ErrorsListWindow : Window
    {
        public ErrorsListWindow(ObservableCollection<Error> errors)
        {
            DataContext = new ErrorsListViewModel(errors);
            InitializeComponent();
        }
    }
}
