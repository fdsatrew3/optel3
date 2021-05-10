using OPTEL.UI.Desktop.Services.WindowClosers;
using OPTEL.UI.Desktop.ViewModels;
using OPTEL.UI.Desktop.Views.Core;

namespace OPTEL.UI.Desktop.Views
{
    /// <summary>
    /// Логика взаимодействия для ExtrudersWindow.xaml
    /// </summary>
    public partial class ExtrudersWindow : DatabaseEntityView
    {
        public ExtrudersWindow()
        {
            InitializeComponent();
            var windowCloseService = new DialogWindowCloseService(this);
            this.DataContext = new ExtrudersViewModel(windowCloseService);
        }

        public override void SetSelectedItem(object item)
        {
            ElementsList.SelectedItem = item;
        }
    }
}
