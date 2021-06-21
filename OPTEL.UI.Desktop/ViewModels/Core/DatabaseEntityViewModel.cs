using EasyLocalization.Localization;
using OPTEL.UI.Desktop.Helpers;
using OPTEL.UI.Desktop.Models;
using OPTEL.UI.Desktop.Services.ErrorsListWindows.Base;
using OPTEL.UI.Desktop.Services.WindowClosers.Base;
using System;
using System.Collections.Generic;
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
            private set
            {
                _isDataChanged = value;
                OnPropertyChanged("IsDataChanged");
            }
        }
        public bool IsSavingChanges
        {
            get => _isSavingChanges;
            private set
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
        private bool _isIgnoreDataChangedMark;

        private RelayCommand _markEntityDataAsChangedCommand;
        private RelayCommand _saveChangesCommand;
        private RelayCommand _checkForUnsavedChangesOnWindowClosingCommand;
        private RelayCommand _selectFirstDataEntryIfExistsCommand;
        private RelayCommand _addEntityCommand;
        private RelayCommand _removeEntityCommand;
        private RelayCommand _cloneEntityCommand;
        private RelayCommand _ignoreMarkDataChangedRequestsCommand;
        private RelayCommand _acceptMarkDataChangedRequestsCommand;

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
                    if (_isIgnoreDataChangedMark)
                    {
                        return;
                    }
                    IsDataChanged = true;
                });
            }
        }
        public RelayCommand IgnoreMarkDataChangedRequestsCommand
        {
            get
            {
                return _ignoreMarkDataChangedRequestsCommand ??= new RelayCommand(obj =>
                {
                    _isIgnoreDataChangedMark = true;
                });
            }
        }
        public RelayCommand AcceptMarkDataChangedRequestsCommand
        {
            get
            {
                return _acceptMarkDataChangedRequestsCommand ??= new RelayCommand(obj =>
                {
                    _isIgnoreDataChangedMark = false;
                });
            }
        }
        public RelayCommand SaveChangesCommand
        {
            get
            {
                return _saveChangesCommand ??= new RelayCommand(async obj =>
                {
                    bool isThereAreErrors = false;
                    ObservableCollection<Error> customErrors = GetCustomErrors();
                    try
                    {
                        OnSaveChanges();
                    }
                    catch (Exception ex)
                    {
                        PopulateErrorsListWithException(customErrors, ex);
                    }
                    isThereAreErrors = (customErrors.Count > 0);
                    if (isThereAreErrors == true)
                    {
                        ErrorsListService.ShowErrorsListWindow(customErrors);
                        return;
                    }
                    IsSavingChanges = true;
                    try
                    {
                        await Database.instance.SaveAsync();
                    }
                    catch (Exception ex)
                    {
                        PopulateErrorsListWithException(customErrors, ex);
                        ErrorsListService.ShowErrorsListWindow(customErrors);
                        isThereAreErrors = true;
                    }
                    if (!isThereAreErrors)
                    {
                        IsDataChanged = false;
                    }
                    IsSavingChanges = false;
                });
            }
        }

        private void PopulateErrorsListWithException(ObservableCollection<Error> errorsList, Exception ex)
        {
            Exception currentException = ex;
            errorsList.Add(new Error { Content = ex.Message });
            while (currentException.InnerException != null)
            {
                errorsList.Add(new Error { Content = currentException.InnerException.Message });
                currentException = currentException.InnerException;
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

        public RelayCommand SelectFirstDataEntryIfExistsCommand
        {
            get
            {
                return _selectFirstDataEntryIfExistsCommand ??= new RelayCommand(obj =>
                {
                    SelectFirstDataEntryIfExist();
                });
            }
        }

        public RelayCommand AddEntityCommand
        {
            get
            {
                return _addEntityCommand ??= new RelayCommand(obj =>
                {
                    AddEntity();
                    MarkEntityDataAsChangedCommand.Execute(null);
                }, (obj) => AddEntityExecuteCondition());
            }
        }
        public RelayCommand RemoveEntityCommand
        {
            get
            {
                return _removeEntityCommand ??= new RelayCommand(obj =>
                {
                    RemoveEntity();
                    MarkEntityDataAsChangedCommand.Execute(null);
                }, (obj) => RemoveEntityExecuteCondition());
            }
        }
        public RelayCommand CloneEntityCommand
        {
            get
            {
                return _cloneEntityCommand ??= new RelayCommand(obj =>
                {
                    CloneEntity();
                    MarkEntityDataAsChangedCommand.Execute(null);
                }, (obj) => CloneEntityExecuteCondition());
            }
        }
        #endregion

        public virtual ObservableCollection<Error> GetCustomErrors()
        {
            return new ObservableCollection<Error>();
        }

        public virtual void SelectFirstDataEntryIfExist()
        {

        }

        public virtual void AddEntity()
        {

        }
        public virtual bool AddEntityExecuteCondition()
        {
            return true;
        }

        public virtual void RemoveEntity()
        {

        }
        public virtual bool RemoveEntityExecuteCondition()
        {
            return false;
        }

        public virtual void CloneEntity()
        {

        }
        public virtual bool CloneEntityExecuteCondition()
        {
            return false;
        }

        public virtual void OnSaveChanges()
        {

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
