using EasyLocalization.Localization;
using OPTEL.UI.Desktop.Helpers;
using OPTEL.UI.Desktop.Services.WindowClosers.Base;
using System;
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
        #endregion

        #region Fields
        private bool _isDataChanged;
        private bool _isSavingChanges;

        private RelayCommand _markEntityDataAsChangedCommand;
        private RelayCommand _saveChangesCommand;
        private RelayCommand _checkForUnsavedChangesOnWindowClosingCommand;

        private IWindowCloseService _windowCloseService;
        #endregion
        public DatabaseEntityViewModel(IWindowCloseService windowCloseService)
        {
            IsDataChanged = false;
            IsSavingChanges = false;
            _windowCloseService = windowCloseService;
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
                    _windowCloseService.SetAllowWindowClosing(false);
                    bool error = false;
                    string customError = GetCustomErrorString();
                    if (customError.Length > 0)
                    {
                        error = true;
                    }
                    if (error == true)
                    {
                        MessageBox.Show(customError,
                            LocalizationManager.Instance.GetValue("Window.Global.MessageBox.Error.Global.Title"),
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                    else
                    {
                        IsSavingChanges = true;
                        try
                        {
                            await Database.instance.SaveAsync();
                        }
                        catch (Exception ex)
                        {
                            Exception currentException = ex;
                            string errorMessage = ex.Message;
                            while (currentException.InnerException != null)
                            {
                                errorMessage += "\n" + currentException.InnerException.Message;
                                currentException = currentException.InnerException;
                            }
                            MessageBox.Show(errorMessage,
                                LocalizationManager.Instance.GetValue("Window.Global.MessageBox.Error.Global.Title"),
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                            error = true;
                        }
                        if (!error)
                        {
                            IsDataChanged = false;
                        }
                        IsSavingChanges = false;
                        _windowCloseService.SetAllowWindowClosing(true);
                    }
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
                        _windowCloseService.SetAllowWindowClosing(true);
                    }
                });
            }
        }
        #endregion

        public virtual string GetCustomErrorString()
        {
            return string.Empty;
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
