using OPTEL.UI.Desktop.ViewModels;
using OPTEL.UI.Desktop.Views.Core;

namespace OPTEL.UI.Desktop.Views
{
    /// <summary>
    /// Логика взаимодействия для ExtruderCoolingLipWindow.xaml
    /// </summary>
    public partial class ExtruderRecipesChangeWindow : DatabaseEntityView
    {
        public ExtruderRecipesChangeWindow()
        {
            InitializeComponent();
            this.DataContext = new ExtruderRecipesChangeViewModel();
        }

        public override void SetSelectedItem(object item)
        {
            ElementsList.SelectedItem = item;
        }
    }
}
