using OPTEL.UI.Desktop.Models;
using System.Collections.ObjectModel;

namespace OPTEL.UI.Desktop.Services.ErrorsListWindows.Base
{
    public interface IErrorsListWindowService
    {
        public void ShowErrorsListWindow(ObservableCollection<Error> errors);
    }
}
