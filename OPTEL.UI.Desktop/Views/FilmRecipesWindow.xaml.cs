using OPTEL.UI.Desktop.Services.WindowClosers;
using OPTEL.UI.Desktop.ViewModels;
using OPTEL.UI.Desktop.Views.Core;

namespace OPTEL.UI.Desktop.Views
{
    /// <summary>
    /// Логика взаимодействия для FilmRecipesWindow.xaml
    /// </summary>
    public partial class FilmRecipesWindow : DatabaseEntityView
    {
        public FilmRecipesWindow()
        {
            InitializeComponent();
            var windowCloseService = new DialogWindowCloseService(this);
            this.DataContext = new FilmRecipesViewModel(windowCloseService);
        }

        public override void SetSelectedItem(object item)
        {
            ElementsList.SelectedItem = item;
        }
    }
}
