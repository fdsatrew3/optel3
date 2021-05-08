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
        #region Fields
        private RelayCommand _openExtruderCalibrationsWindowCommand;
        private RelayCommand _openExtruderNozzlesWindowCommand;
        private RelayCommand _openExtruderCoolingLipsWindowCommand;
        private RelayCommand _openExtruderRecipesWindowCommand;
        private RelayCommand _openExtrudersWindowCommand;
        private RelayCommand _openFilmTypesWindowCommand;
        private RelayCommand _openFilmRecipesWindowCommand;
        private RelayCommand _openCustomersWindowCommand;
        private RelayCommand _openOrdersWindowCommand;
        private RelayCommand _openImportExcelWindowCommand;
        #endregion
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
        #region Commands
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
                    ExtruderCoolingLipsWindow window = new ExtruderCoolingLipsWindow();
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

        public RelayCommand OpenFilmTypesWindowCommand
        {
            get
            {
                return _openFilmTypesWindowCommand ??= new RelayCommand(obj =>
                {
                    FilmTypesWindow window = new FilmTypesWindow();
                    window.ShowModalDialog();
                });
            }
        }

        public RelayCommand OpenFilmRecipesWindowCommand
        {
            get
            {
                return _openFilmRecipesWindowCommand ??= new RelayCommand(obj =>
                {
                    FilmRecipesWindow window = new FilmRecipesWindow();
                    window.ShowModalDialog();
                });
            }
        }
        public RelayCommand OpenCustomersWindowCommand
        {
            get
            {
                return _openCustomersWindowCommand ??= new RelayCommand(obj =>
                {
                    CustomersWindow window = new CustomersWindow();
                    window.ShowModalDialog();
                });
            }
        }
        public RelayCommand OpenOrdersWindowCommand
        {
            get
            {
                return _openOrdersWindowCommand ??= new RelayCommand(obj =>
                {
                    OrdersWindow window = new OrdersWindow();
                    window.ShowModalDialog();
                });
            }
        }
        public RelayCommand OpenImportExcelWindowCommand
        {
            get
            {
                return _openImportExcelWindowCommand ??= new RelayCommand(obj =>
                {
                    ImportExcelDataWindow window = new ImportExcelDataWindow();
                    window.ShowModalDialog();
                });
            }
        }
        #endregion
    }
}
