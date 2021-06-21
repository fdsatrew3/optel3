using OPTEL.Data.Users;
using OPTEL.UI.Desktop.Services.ErrorsListWindows;
using OPTEL.UI.Desktop.Services.ModelsConverter;
using OPTEL.UI.Desktop.Services.WindowClosers;
using OPTEL.UI.Desktop.ViewModels;
using OPTEL.UI.Desktop.Views.Core;

namespace OPTEL.UI.Desktop.Views
{
    /// <summary>
    /// Логика взаимодействия для CustomersWindow.xaml
    /// </summary>
    public partial class ProductionDirectorsWindow : DatabaseEntityView
    {
        public ProductionDirectorsWindow()
        {
            InitializeComponent();
            var windowCloseService = new GenericWindowCloseService(this);
            var errorsListWindowService = new GenericErrorListWindowService(this);
            var userConverterService = new UserToDataUserConverterService<ProductionDirector>();
            DataContext = new ProductionDirectorsViewModel(windowCloseService, errorsListWindowService, userConverterService);
        }

        public override void SetSelectedItem(object item)
        {
            ElementsList.SelectedItem = item;
        }
    }
}
