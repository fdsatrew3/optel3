﻿using EasyLocalization.Localization;
using OPTEL.UI.Desktop.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
        public bool IsCloseAllowed { get => _isCloseAllowed; set => _isCloseAllowed = value; }
        #endregion

        #region Fields
        private bool _isDataChanged;
        private bool _isSavingChanges;
        private bool _isCloseAllowed;

        private RelayCommand _markEntityDataAsChanged;
        private RelayCommand _saveChanges;
        private RelayCommand _checkForUnsavedChangesOnWindowClosing;
        #endregion

        #region Commands
        public RelayCommand MarkEntityDataAsChanged
        {
            get
            {
                return _markEntityDataAsChanged ??= new RelayCommand(obj =>
                {
                    IsDataChanged = true;
                });
            }
        }
        public RelayCommand SaveChanges
        {
            get
            {
                return _saveChanges ??= new RelayCommand(async obj =>
                {
                    bool error = false;
                    string customError = CheckForCustomErrors();
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
                    }
                });
            }
        }
        public RelayCommand CheckForUnsavedChangesOnWindowClosing
        {
            get
            {
                return _checkForUnsavedChangesOnWindowClosing ??= new RelayCommand(obj =>
                {
                    if (IsSavingChanges == true)
                    {
                        MessageBox.Show(LocalizationManager.Instance.GetValue("Window.Global.MessageBox.Error.SavingChanges.Content"),
                            LocalizationManager.Instance.GetValue("Window.Global.MessageBox.Error.Global.Title"),
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        return;
                    }
                    if (IsDataChanged == true)
                    {
                        MessageBoxResult result = MessageBox.Show(
                            LocalizationManager.Instance.GetValue("Window.Global.MessageBox.Warning.UnsavedChanges.Content"),
                            LocalizationManager.Instance.GetValue("Window.Global.MessageBox.Warning.Global.Title"),
                            MessageBoxButton.YesNoCancel,
                            MessageBoxImage.Warning);
                        if (result == MessageBoxResult.Yes)
                        {
                            SaveChanges.Execute(null);
                            return;
                        }
                        if (result == MessageBoxResult.No)
                        {
                            Database.instance.RejectAllChanges();
                        }
                        if (result == MessageBoxResult.Cancel)
                        {
                            return;
                        }
                    }
                    IsCloseAllowed = true;
                });
            }
        }
        #endregion

        public DatabaseEntityViewModel()
        {
            IsDataChanged = false;
            IsSavingChanges = false;
            IsCloseAllowed = false;
        }

        public virtual string CheckForCustomErrors()
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
