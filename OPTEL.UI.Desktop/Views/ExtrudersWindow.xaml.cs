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
            this.DataContext = new ExtrudersViewModel();
        }

        public override void SetSelectedItem(object item)
        {
            ElementsList.SelectedItem = item;
        }
    }
}
