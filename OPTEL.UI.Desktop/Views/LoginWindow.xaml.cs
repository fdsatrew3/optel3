using EasyLocalization.Localization;
using OPTEL.UI.Desktop.Services.WindowClosers;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace OPTEL.UI.Desktop.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// Bad implementation, but fuck it ._.
    /// </summary>
    public partial class LoginWindow : Window
    {
        private static SHA256 _crypto = SHA256.Create();
        private bool _success;
        public LoginWindow()
        {
            InitializeComponent();
            new GenericWindowDragDisablerService(this);
            Closing += new CancelEventHandler(OnWindowClose);
            var count = Database.instance.UserRepository.GetCount();
            textWarning.Visibility = Visibility.Collapsed;
            if (count == 1)
            {
                var firstUser = Database.instance.UserRepository.GetFirst();
                if (firstUser.Login.Equals("admin") && firstUser.Password.Equals(Encrypt("admin")))
                {
                    textWarning.Visibility = Visibility.Visible;
                }
            }
        }
        private void OnWindowClose(object sender, CancelEventArgs e)
        {
            if (_success)
            {
                return;
            }
            Application.Current.Shutdown();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (loginTextBox.Text.Length < 4)
            {
                MessageBox.Show(this, LocalizationManager.Instance.GetValue("Window.Login.Errors.LoginIsTooShort"), LocalizationManager.Instance.GetValue("Window.Global.MessageBox.Error.Global.Title"), MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            bool userExists = false;
            foreach (var user in Database.instance.UserRepository.GetAll())
            {
                if (user.Login.Equals(loginTextBox.Text) && user.Password.Equals(Encrypt(passwordBox.Password)))
                {
                    userExists = true;
                    break;
                }
            }
            if (userExists)
            {
                _success = true;
                this.Close();
            }
            else
            {
                MessageBox.Show(this, LocalizationManager.Instance.GetValue("Window.Login.Errors.CredentialsAreNotFound"), LocalizationManager.Instance.GetValue("Window.Global.MessageBox.Error.Global.Title"), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static string Encrypt(string text)
        {
            var bytes = _crypto.ComputeHash(Encoding.UTF8.GetBytes(text));
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
