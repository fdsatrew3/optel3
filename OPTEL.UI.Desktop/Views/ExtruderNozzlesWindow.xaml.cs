using OPTEL.UI.Desktop.ViewModels;
using OPTEL.UI.Desktop.Views.Core;

namespace OPTEL.UI.Desktop.Views
{
    /// <summary>
    /// Логика взаимодействия для ExtruderNozzlesWindow.xaml
    /// </summary>
    public partial class ExtruderNozzlesWindow : DatabaseEntityView
    {
        public ExtruderNozzlesWindow()
        {
            InitializeComponent();
            this.DataContext = new ExtruderNozzlesViewModel();
        }

        public override void SetSelectedItem(object item)
        {
            ElementsList.SelectedItem = item;
        }
    }
}
