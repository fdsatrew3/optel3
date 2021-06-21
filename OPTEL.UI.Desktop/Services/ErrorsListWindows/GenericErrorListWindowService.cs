using EasyLocalization.Localization;
using OPTEL.UI.Desktop.Helpers;
using OPTEL.UI.Desktop.Models;
using OPTEL.UI.Desktop.Services.ErrorsListWindows.Base;
using OPTEL.UI.Desktop.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace OPTEL.UI.Desktop.Services.ErrorsListWindows
{
    public class GenericErrorListWindowService : IErrorsListWindowService
    {
        private Window _parent;

        public GenericErrorListWindowService(Window parent)
        {
            _parent = parent;
        }

        public void ShowErrorsListWindow(ObservableCollection<Error> errors)
        {
            ErrorsListWindow window = new ErrorsListWindow(errors);
            window.ShowModalDialog(_parent);
        }
    }
}
