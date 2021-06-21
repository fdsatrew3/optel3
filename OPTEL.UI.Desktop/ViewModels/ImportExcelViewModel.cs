using EasyLocalization.Localization;
using OPTEL.UI.Desktop.Helpers;
using OPTEL.UI.Desktop.Models;
using OPTEL.UI.Desktop.Services.ErrorsListWindows.Base;
using OPTEL.UI.Desktop.Services.ExcelDataReaders.Base;
using OPTEL.UI.Desktop.Services.OpenFileDialogs.Base;
using OPTEL.UI.Desktop.Services.WindowClosers.Base;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OPTEL.UI.Desktop.ViewModels
{
    public class ImportExcelViewModel : INotifyPropertyChanged
    {
        #region Fields
        private bool _isImportingData;
        private bool _isCloseAllowed;

        private bool? _isDeleteAllCheckBoxChecked;

        private string _selectedExcelFile;
        private string _progressBarText;

        private IOpenFileDialogService _openFileDialogService;

        private IExcelEntityReader _excelReaderService;

        private IWindowCloseService _windowCloseService;

        private IErrorsListWindowService _errorsListWindowService;

        private RelayCommand _openFileDialogCommand;
        private RelayCommand _beginExcelFileImportCommand;
        #endregion

        #region Properties
        public bool IsImportingData
        {
            get => _isImportingData;
            set
            {
                _isImportingData = value;
                OnPropertyChanged("IsImportingData");
            }
        }
        public string SelectedExcelFile
        {
            get => _selectedExcelFile;
            set
            {
                _selectedExcelFile = value;
                OnPropertyChanged("SelectedExcelFile");
            }
        }
        public string ProgressBarText
        {
            get => _progressBarText;
            set
            {
                _progressBarText = value;
                OnPropertyChanged("ProgressBarText");
            }
        }
        public bool? IsDeleteAllCheckBoxChecked
        {
            get => (_isDeleteAllCheckBoxChecked != null) ? _isDeleteAllCheckBoxChecked : false;
            set
            {
                _isDeleteAllCheckBoxChecked = value;
                OnPropertyChanged("IsDeleteAllCheckBoxChecked");
            }
        }

        public bool IsCloseAllowed { get => _isCloseAllowed; set => _isCloseAllowed = value; }
        #endregion
        public ImportExcelViewModel(IOpenFileDialogService openFileDialogService, IExcelEntityReader excelReaderService, IWindowCloseService windowCloseService, IErrorsListWindowService errorsListWindowService)
        {
            _openFileDialogService = openFileDialogService;
            _excelReaderService = excelReaderService;
            _windowCloseService = windowCloseService;
            _errorsListWindowService = errorsListWindowService;
            IsImportingData = false;
            IsDeleteAllCheckBoxChecked = false;
            ProgressBarText = LocalizationManager.Instance.GetValue("Window.ImportExcel.ImportingData");
        }

        #region Commands
        public RelayCommand OpenFileDialogCommand
        {
            get
            {
                return _openFileDialogCommand ??= new RelayCommand(obj =>
                {
                    string fileName = _openFileDialogService.ShowOpenFileDialog();
                    if (fileName != null)
                    {
                        SelectedExcelFile = fileName;
                    }
                });
            }
        }
        public RelayCommand BeginExcelFileImportCommand
        {
            get
            {
                return _beginExcelFileImportCommand ??= new RelayCommand(async obj =>
                {
                    _windowCloseService.SetAllowWindowClosing(false);
                    _windowCloseService.SetReasonMessage(LocalizationManager.Instance.GetValue("Window.ImportExcel.Errors.ClosingWhileImporting"));
                    if (SelectedExcelFile == null)
                    {
                        IsImportingData = false;
                        _windowCloseService.SetAllowWindowClosing(true);
                        MessageBox.Show(
                            LocalizationManager.Instance.GetValue("Window.ImportExcel.Errors.SelectedExcelFileIsNull"),
                            LocalizationManager.Instance.GetValue("Window.Global.MessageBox.Error.Global.Title"),
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        return;
                    }
                    if (IsDeleteAllCheckBoxChecked == true)
                    {
                        MessageBoxResult result = MessageBox.Show(
                            LocalizationManager.Instance.GetValue("Window.ImportExcel.Warnings.DeleteAllDataFromDatabase"),
                            LocalizationManager.Instance.GetValue("Window.Global.MessageBox.Warning.Global.Title"),
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Warning);
                        if (result == MessageBoxResult.No)
                        {
                            return;
                        }
                        IsImportingData = true;
                        ProgressBarText = LocalizationManager.Instance.GetValue("Window.ImportExcel.DeletingData");
                        await Task.Run(() =>
                        {
                            Database.instance.CalibrationChangeRepository.DeleteRange(Database.instance.CalibrationChangeRepository.GetAll());
                            Database.instance.CoolingLipChangeRepository.DeleteRange(Database.instance.CoolingLipChangeRepository.GetAll());
                            Database.instance.CustomerRepository.DeleteRange(Database.instance.CustomerRepository.GetAll());
                            Database.instance.FilmRecipeChangeRepository.DeleteRange(Database.instance.FilmRecipeChangeRepository.GetAll());
                            Database.instance.FilmRecipeRepository.DeleteRange(Database.instance.FilmRecipeRepository.GetAll());
                            Database.instance.FilmTypeRepository.DeleteRange(Database.instance.FilmTypeRepository.GetAll());
                            Database.instance.NozzleChangeRepository.DeleteRange(Database.instance.NozzleChangeRepository.GetAll());
                            Database.instance.OrderRepository.DeleteRange(Database.instance.OrderRepository.GetAll());
                            Database.instance.ProductionLineRepository.DeleteRange(Database.instance.ProductionLineRepository.GetAll());
                            Database.instance.Save();
                        });
                        ProgressBarText = LocalizationManager.Instance.GetValue("Window.ImportExcel.ImportingData");
                    } else
                    {
                        IsImportingData = true;
                    }
                    bool error = false;
                    ObservableCollection<Error> errors = new ObservableCollection<Error>();
                    Exception currentException = null;
                    try
                    {
                        await Task.Run(() =>
                            {
                                foreach (var actionResult in _excelReaderService.ReadFile(SelectedExcelFile))
                                {
                                    if (actionResult.HasException)
                                    {
                                        error = true;
                                        currentException = actionResult.Exception;
                                        while (currentException != null)
                                        {
                                            errors.Add(new Error
                                            {
                                                Content = currentException.Message
                                            });
                                            currentException = currentException.InnerException;
                                        }
                                    }
                                }
                            });
                    }
                    catch (Exception ex)
                    {
                        currentException = ex;
                        while (currentException != null)
                        {
                            errors.Add(new Error
                            {
                                Content = currentException.Message
                            });
                            currentException = currentException.InnerException;
                        }
                    }
                    _windowCloseService.SetAllowWindowClosing(true);
                    IsImportingData = false;
                    if (error)
                    {
                        Database.instance.RejectAllChanges();
                        _errorsListWindowService.ShowErrorsListWindow(errors);
                        return;
                    }
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
