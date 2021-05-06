using EasyLocalization.Localization;
using OPTEL.UI.Desktop.Helpers;
using System.Windows;

namespace OPTEL.UI.Desktop.View
{
    /// <summary>
    /// Логика взаимодействия для ExtruderCalibrationsWindow.xaml
    /// </summary>
    public partial class ExtruderCalibrationsWindow : Window
    {
        private int _index = 0;
        public ExtruderCalibrationsWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var availableCultures = LocalizationManager.Instance.AvailableCultures;
            _index++;

            if (_index == availableCultures.Count)
                _index = 0;

            LocalizationManager.Instance.CurrentCulture = availableCultures[_index];
        }
    }
}
