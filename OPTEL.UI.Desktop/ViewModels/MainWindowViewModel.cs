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
        private RelayCommand _openExtruderCalibrationsWindowCommand;
        private RelayCommand _openExtruderNozzlesWindowCommand;
        private RelayCommand _openExtruderCoolingLipsWindowCommand;
        private RelayCommand _openExtruderRecipesWindowCommand;
        private RelayCommand _openExtrudersWindowCommand;
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
        public RelayCommand OpenExtruderCoolingLipsWindowCommand
        {
            get
            {
                return _openExtruderCoolingLipsWindowCommand ??= new RelayCommand(obj =>
                {
                    ExtruderCoolingLipWindow window = new ExtruderCoolingLipWindow();
                    window.ShowModalDialog();
                });
            }
        }
        public RelayCommand OpenExtruderRecipesWindowCommand
        {
            get
            {
                return _openExtruderRecipesWindowCommand ??= new RelayCommand(obj =>
                {
                    ExtruderRecipesChangeWindow window = new ExtruderRecipesChangeWindow();
                    window.ShowModalDialog();
                });
            }
        }
        public RelayCommand OpenExtrudersWindowCommand
        {
            get
            {
                return _openExtrudersWindowCommand ??= new RelayCommand(obj =>
                {
                    ExtrudersWindow window = new ExtrudersWindow();
                    window.ShowModalDialog();
                });
            }
        }
    }
}
