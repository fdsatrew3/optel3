namespace OPTEL.UI.Desktop.Services.WindowClosers.Base
{
    public interface IWindowCloseService
    {
        public void SetReasonMessage(string msg);
        public void SetAllowWindowClosing(bool allow);
        public bool IsCloseAllowed();
        public void CloseWindow();
    }
}
