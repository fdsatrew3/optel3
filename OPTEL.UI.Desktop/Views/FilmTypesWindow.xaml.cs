using OPTEL.UI.Desktop.ViewModels;
using OPTEL.UI.Desktop.Views.Core;

namespace OPTEL.UI.Desktop.Views
{
    /// <summary>
    /// Логика взаимодействия для FilmTypesWindow.xaml
    /// </summary>
    public partial class FilmTypesWindow : DatabaseEntityView
    {
        public FilmTypesWindow()
        {
            InitializeComponent();
            this.DataContext = new FilmTypesViewModel();
        }

        public override void SetSelectedItem(object item)
        {
            ElementsList.SelectedItem = item;
        }
    }
}
