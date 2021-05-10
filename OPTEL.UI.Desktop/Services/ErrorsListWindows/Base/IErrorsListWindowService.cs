using OPTEL.UI.Desktop.Models;
using System.Collections.ObjectModel;

namespace OPTEL.UI.Desktop.Services.ErrorsListWindows.Base
{
    public interface IErrorsListWindowService
    {
        public void SetErrorsForDisplay(ObservableCollection<Error> errors);
        public void ShowErrorsListWindow();
    }
}
