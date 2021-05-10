using OPTEL.UI.Desktop.Services.WindowClosers;
using OPTEL.UI.Desktop.ViewModels;
using OPTEL.UI.Desktop.Views.Core;

namespace OPTEL.UI.Desktop.Views
{
    /// <summary>
    /// Логика взаимодействия для CustomersWindow.xaml
    /// </summary>
    public partial class CustomersWindow : DatabaseEntityView
    {
        public CustomersWindow()
        {
            InitializeComponent();
            var windowCloseService = new DialogWindowCloseService(this);
            this.DataContext = new CustomersViewModel(windowCloseService);
        }

        public override void SetSelectedItem(object item)
        {
            ElementsList.SelectedItem = item;
        }
    }
}
