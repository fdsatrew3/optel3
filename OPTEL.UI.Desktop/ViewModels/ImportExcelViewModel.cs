using EasyLocalization.Localization;
using OPTEL.UI.Desktop.Helpers;
using OPTEL.UI.Desktop.Services.ExcelDataReaders.Base;
using OPTEL.UI.Desktop.Services.OpenFileDialogs.Base;
using System;
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
        private bool? _isDeleteAllCheckBoxChecked;

        private string _selectedExcelFile;

        private IOpenFileDialogService _openFileDialogService;

        private IExcelEntityReader _excelReaderService;

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
        public bool? IsDeleteAllCheckBoxChecked
        {
            get => (_isDeleteAllCheckBoxChecked != null) ? _isDeleteAllCheckBoxChecked : false;
            set
            {
                _isDeleteAllCheckBoxChecked = value;
                OnPropertyChanged("IsDeleteAllCheckBoxChecked");
            }
        }
        #endregion
        public ImportExcelViewModel(IOpenFileDialogService openFileDialogService, IExcelEntityReader excelReaderService)
        {
            _openFileDialogService = openFileDialogService;
            _excelReaderService = excelReaderService;
            IsImportingData = false;
            IsDeleteAllCheckBoxChecked = false;
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
                    IsImportingData = true;
                    if (SelectedExcelFile == null)
                    {
                        IsImportingData = false;
                        MessageBox.Show(
                            LocalizationManager.Instance.GetValue("Window.ImportExcel.Errors.SelectedExcelFileIsNull"),
                            LocalizationManager.Instance.GetValue("Window.Global.MessageBox.Error.Global.Title"),
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        return;
                    }
                    bool error = false;
                    StringBuilder errorMessage = new StringBuilder();
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
                                        errorMessage.AppendLine(currentException.Message);
                                        currentException = currentException.InnerException;
                                    }
                                }
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        error = true;
                        currentException = ex;
                        while (currentException != null)
                        {
                            errorMessage.AppendLine(currentException.Message);
                            currentException = currentException.InnerException;
                        }
                    }
                    IsImportingData = false;
                    if (error)
                    {
                        Database.instance.RejectAllChanges();
                        MessageBox.Show(
                            errorMessage.ToString().Substring(0, 512),
                            LocalizationManager.Instance.GetValue("Window.Global.MessageBox.Error.Global.Title"),
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        Console.WriteLine(1);
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
