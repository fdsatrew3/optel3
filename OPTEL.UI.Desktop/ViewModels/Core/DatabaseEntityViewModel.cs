using EasyLocalization.Localization;
using OPTEL.UI.Desktop.Helpers;
using OPTEL.UI.Desktop.Models;
using OPTEL.UI.Desktop.Services.ErrorsListWindows.Base;
using OPTEL.UI.Desktop.Services.WindowClosers.Base;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace OPTEL.UI.Desktop.ViewModels.Core
{
    public class DatabaseEntityViewModel : INotifyPropertyChanged
    {
        #region Properties
        public bool IsDataChanged
        {
            get => _isDataChanged;
            set
            {
                _isDataChanged = value;
                OnPropertyChanged("IsDataChanged");
            }
        }
        public bool IsSavingChanges
        {
            get => _isSavingChanges;
            set
            {
                _isSavingChanges = value;
                OnPropertyChanged("IsSavingChanges");
            }
        }
        public IErrorsListWindowService ErrorsListService { get => _errorsListService; set => _errorsListService = value; }
        #endregion

        #region Fields
        private bool _isDataChanged;
        private bool _isSavingChanges;

        private RelayCommand _markEntityDataAsChangedCommand;
        private RelayCommand _saveChangesCommand;
        private RelayCommand _checkForUnsavedChangesOnWindowClosingCommand;

        private IWindowCloseService _windowCloseService;

        private IErrorsListWindowService _errorsListService;
        #endregion
        public DatabaseEntityViewModel(IWindowCloseService windowCloseService, IErrorsListWindowService errorsListService)
        {
            IsDataChanged = false;
            IsSavingChanges = false;
            _windowCloseService = windowCloseService;
            _windowCloseService.SetDetermineCloseAllowedCommand(CheckForUnsavedChangesOnWindowClosingCommand);
            _errorsListService = errorsListService;
        }

        #region Commands
        public RelayCommand MarkEntityDataAsChangedCommand
        {
            get
            {
                return _markEntityDataAsChangedCommand ??= new RelayCommand(obj =>
                {
                    IsDataChanged = true;
                });
            }
        }
        public RelayCommand SaveChangesCommand
        {
            get
            {
                return _saveChangesCommand ??= new RelayCommand(async obj =>
                {
                    bool error = false;
                    ObservableCollection<Error> customErrors = GetCustomErrors();
                    if (customErrors.Count > 0)
                    {
                        error = true;
                    }
                    if (error == true)
                    {
                        ErrorsListService.SetErrorsForDisplay(customErrors);
                        ErrorsListService.ShowErrorsListWindow();
                        return;
                    }
                    IsSavingChanges = true;
                    try
                    {
                        await Database.instance.SaveAsync();
                    }
                    catch (Exception ex)
                    {
                        Exception currentException = ex;
                        customErrors.Add(new Error { Content = ex.Message });
                        while (currentException.InnerException != null)
                        {
                            customErrors.Add(new Error { Content = currentException.InnerException.Message });
                            currentException = currentException.InnerException;
                        }
                        ErrorsListService.ShowErrorsListWindow();
                        error = true;
                    }
                    if (!error)
                    {
                        IsDataChanged = false;
                    }
                    IsSavingChanges = false;
                });
            }
        }
        public RelayCommand CheckForUnsavedChangesOnWindowClosingCommand
        {
            get
            {
                return _checkForUnsavedChangesOnWindowClosingCommand ??= new RelayCommand(obj =>
                {
                    _windowCloseService.SetAllowWindowClosing(false);
                    if (IsSavingChanges == true)
                    {
                        _windowCloseService.SetReasonMessage("Window.Global.MessageBox.Error.SavingChanges");
                        return;
                    }
                    if (IsDataChanged == true)
                    {
                        MessageBoxResult result = MessageBox.Show(
                            LocalizationManager.Instance.GetValue("Window.Global.MessageBox.Warning.UnsavedChanges"),
                            LocalizationManager.Instance.GetValue("Window.Global.MessageBox.Warning.Global.Title"),
                            MessageBoxButton.YesNoCancel,
                            MessageBoxImage.Warning);
                        if (result == MessageBoxResult.Yes)
                        {
                            SaveChangesCommand.Execute(null);
                            return;
                        }
                        if (result == MessageBoxResult.No)
                        {
                            IsDataChanged = false;
                            Database.instance.RejectAllChanges();
                        }
                        if (result == MessageBoxResult.Cancel)
                        {
                            return;
                        }
                    }
                    _windowCloseService.SetAllowWindowClosing(true);
                });
            }
        }
        #endregion

        public virtual ObservableCollection<Error> GetCustomErrors()
        {
            return new ObservableCollection<Error>();
        }

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
