using OPTEL.UI.Desktop.Services.ErrorsListWindows;
using OPTEL.UI.Desktop.Services.WindowClosers;
using OPTEL.UI.Desktop.ViewModels;
using OPTEL.UI.Desktop.Views.Core;

namespace OPTEL.UI.Desktop.Views
{
    /// <summary>
    /// Логика взаимодействия для ExtruderCalibrationsWindow.xaml
    /// </summary>
    public partial class ExtruderCalibrationsWindow : DatabaseEntityView
    {
        public ExtruderCalibrationsWindow()
        {
            InitializeComponent();
            var windowCloseService = new GenericWindowCloseService(this);
            var errorsListWindowService = new GenericErrorListWindowService(this);
            DataContext = new ExtruderCalibrationsViewModel(windowCloseService, errorsListWindowService);
        }

        public override void SetSelectedItem(object item)
        {
            ElementsList.SelectedItem = item;
        }
    }
}
