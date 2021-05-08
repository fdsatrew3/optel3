using OPTEL.UI.Desktop.Helpers;
using OPTEL.UI.Desktop.Services.OpenFileDialogs.Base;
using System.ComponentModel;

namespace OPTEL.UI.Desktop.ViewModels
{
    public class ImportExcelViewModel : INotifyPropertyChanged
    {
        #region Fields
        private bool _isImportingData;
        private bool? _isDeleteAllCheckBoxChecked;

        private string _selectedExcelFile;

        private IOpenFileDialogService _openFileDialogService;

        private RelayCommand _openFileDialogCommand;
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
        public ImportExcelViewModel(IOpenFileDialogService openFileDialogService)
        {
            _openFileDialogService = openFileDialogService;
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
