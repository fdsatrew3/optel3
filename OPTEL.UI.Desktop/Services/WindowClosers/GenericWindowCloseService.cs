using EasyLocalization.Localization;
using OPTEL.UI.Desktop.Helpers;
using OPTEL.UI.Desktop.Services.WindowClosers.Base;
using System.ComponentModel;
using System.Windows;

namespace OPTEL.UI.Desktop.Services.WindowClosers
{
    public class GenericWindowCloseService : IWindowCloseService
    {
        private Window _parent;

        private string _message;

        private bool _isCloseAllowed;

        private RelayCommand _determineCloseAllowedCommand;

        public GenericWindowCloseService(Window parent)
        {
            _parent = parent;
            _isCloseAllowed = true;
            _message = string.Empty;
            _parent.Closing += new CancelEventHandler(OnWindowClose);
        }

        private void OnWindowClose(object sender, CancelEventArgs e)
        {
            if (_determineCloseAllowedCommand != null)
            {
                _determineCloseAllowedCommand.Execute(null);
            }
            if (!IsCloseAllowed())
            {
                e.Cancel = true;
                if (_message.Length > 0)
                {
                    MessageBox.Show(_parent,
                    LocalizationManager.Instance.GetValue(_message),
                    LocalizationManager.Instance.GetValue("Window.Global.MessageBox.Error.Global.Title"),
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                }
            }
            else
            {
                e.Cancel = false;
            }
        }

        public void CloseWindow()
        {
            _isCloseAllowed = true;
            _parent.Closing -= OnWindowClose;
            _parent.DialogResult = true;
            _parent.Close();
        }

        public bool IsCloseAllowed()
        {
            return _isCloseAllowed;
        }

        public void SetAllowWindowClosing(bool allow)
        {
            _isCloseAllowed = allow;
            if (allow)
            {
                SetReasonMessage(string.Empty);
            }
        }

        public void SetReasonMessage(string msg)
        {
            _message = msg;
        }

        public void SetDetermineCloseAllowedCommand(RelayCommand command)
        {
            _determineCloseAllowedCommand = command;
        }
    }
}
