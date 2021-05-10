using OPTEL.UI.Desktop.Helpers;

namespace OPTEL.UI.Desktop.Services.WindowClosers.Base
{
    public interface IDatabaseEntityWindowCloseService : IWindowCloseService
    {
        public void SetCheckForUnsavedChangesCommand(RelayCommand command);
    }
}
