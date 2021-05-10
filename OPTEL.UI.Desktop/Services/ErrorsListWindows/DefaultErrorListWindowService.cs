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
    public class DefaultErrorListWindowService : IErrorsListWindowService
    {
        private Window _parent;
        private ObservableCollection<Error> _errors;

        public DefaultErrorListWindowService(Window parent)
        {
            _parent = parent;
            _errors = new ObservableCollection<Error>();
        }

        public void SetErrorsForDisplay(ObservableCollection<Error> errors)
        {
            _errors = errors;
        }

        public void ShowErrorsListWindow()
        {
            ErrorsListWindow window = new ErrorsListWindow(_errors);
            window.ShowModalDialog(_parent);
        }
    }
}
