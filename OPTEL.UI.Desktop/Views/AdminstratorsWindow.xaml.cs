using OPTEL.UI.Desktop.Services.ErrorsListWindows;
using OPTEL.UI.Desktop.Services.WindowClosers;
using OPTEL.UI.Desktop.ViewModels;
using OPTEL.UI.Desktop.Views.Core;

namespace OPTEL.UI.Desktop.Views
{
    /// <summary>
    /// Логика взаимодействия для CustomersWindow.xaml
    /// </summary>
    public partial class AdminstratorsWindow : DatabaseEntityView
    {
        public AdminstratorsWindow()
        {
            InitializeComponent();
            var windowCloseService = new DefaultWindowCloseService(this);
            var errorsListWindowService = new DefaultErrorListWindowService(this);
            DataContext = new AdministratorsViewModel(windowCloseService, errorsListWindowService);
        }

        public override void SetSelectedItem(object item)
        {
            ElementsList.SelectedItem = item;
        }
    }
}
