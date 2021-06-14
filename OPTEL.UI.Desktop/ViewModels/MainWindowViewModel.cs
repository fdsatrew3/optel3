using EasyLocalization.Localization;
using OPTEL.UI.Desktop.Helpers;
using OPTEL.UI.Desktop.Views;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace OPTEL.UI.Desktop.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Properties
        public object TargetFunctionValue
        {
            get => _targetfunctionValue;
            set
            {
                _targetfunctionValue = value;
                OnPropertyChanged("TargetFunctionValue");
            }
        }

        #endregion
        #region Fields
        private object _targetfunctionValue;

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
        private RelayCommand _openPlanningWindowCommand;
        private RelayCommand _openAdminstratorsWindowCommand;
        private RelayCommand _openKnowledgeEngineersWindowCommand;
        private RelayCommand _openProductionDirectorsWindowCommand;
        private RelayCommand _logoutCommand;
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
                    LocalizationManager.Instance.CurrentCulture = LocalizationManager.Instance.AvailableCultures[Convert.ToInt32((o as MenuItem).Tag)];
                };
                languageMenuItem.Tag = i;
                (Application.Current.MainWindow as MainWindow).languageMenuItem.Items.Add(languageMenuItem);
            }
            LocalizationManager.Instance.PropertyChanged += (o, e) =>
            {
                TargetFunctionValue = TargetFunctionValue;
            };
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

        public RelayCommand OpenPlanningWindowCommand
        {
            get
            {
                return _openPlanningWindowCommand ??= new RelayCommand(obj =>
                {
                    PlanningConfigWindow window = new PlanningConfigWindow();
                    window.ShowModalDialog();
                });
            }
        }
        public RelayCommand OpenAdminstratorsWindowCommand
        {
            get
            {
                return _openAdminstratorsWindowCommand ??= new RelayCommand(obj =>
                {
                    AdminstratorsWindow window = new AdminstratorsWindow();
                    window.ShowModalDialog();
                });
            }
        }
        public RelayCommand OpenKnowledgeEngineersWindowCommand
        {
            get
            {
                return _openKnowledgeEngineersWindowCommand ??= new RelayCommand(obj =>
                {
                    KnowledgeEngineersWindow window = new KnowledgeEngineersWindow();
                    window.ShowModalDialog();
                });
            }
        }
        public RelayCommand OpenProductionDirectorsWindowCommand
        {
            get
            {
                return _openProductionDirectorsWindowCommand ??= new RelayCommand(obj =>
                {
                    ProductionDirectorsWindow window = new ProductionDirectorsWindow();
                    window.ShowModalDialog();
                });
            }
        }
        public RelayCommand LogoutCommand
        {
            get
            {
                return _logoutCommand ??= new RelayCommand(obj =>
                {
                    LoginWindow window = new LoginWindow();
                    window.ShowModalDialog();
                });
            }
        }
        #endregion

        #region PropertyChangedEventHandler
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
