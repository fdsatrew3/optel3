using EasyLocalization.Localization;
using OPTEL.UI.Desktop.Helpers;
using OPTEL.UI.Desktop.Views;
using System;
using System.Windows;
using System.Windows.Controls;

namespace OPTEL.UI.Desktop.ViewModels
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            var availableCultures = LocalizationManager.Instance.AvailableCultures;
            for (var i = 0; i < availableCultures.Count; i++)
            {
                MenuItem languageMenuItem = new MenuItem();
                languageMenuItem.Header = availableCultures[i].EnglishName;
                languageMenuItem.Click += (o, e) =>
                {
                    LocalizationManager.Instance.CurrentCulture = LocalizationManager.Instance.AvailableCultures[Convert.ToInt32((o as MenuItem).DataContext)];
                };
                languageMenuItem.DataContext = i;
                (Application.Current.MainWindow as MainWindow).languageMenuItem.Items.Add(languageMenuItem);
            }
        }

        private RelayCommand _openExtruderCalibrationsWindowCommand;

        public RelayCommand OpenExtruderCalibrationsWindowCommand
        {
            get
            {
                return _openExtruderCalibrationsWindowCommand ??= new RelayCommand(obj =>
                                  {
                                      ExtruderCalibrationsWindow window = new ExtruderCalibrationsWindow();
                                      window.ShowModalDialog();
                                  });
            }
        }

        private RelayCommand _openExtruderNozzlesWindowCommand;

        public RelayCommand OpenExtruderNozzlesWindowCommand
        {
            get
            {
                return _openExtruderNozzlesWindowCommand ??= new RelayCommand(obj =>
                {
                    ExtruderNozzlesWindow window = new ExtruderNozzlesWindow();
                    window.ShowModalDialog();
                });
            }
        }
    }
}
