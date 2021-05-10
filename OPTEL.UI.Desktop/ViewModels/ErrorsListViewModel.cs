using EasyLocalization.Localization;
using OPTEL.UI.Desktop.Helpers;
using OPTEL.UI.Desktop.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace OPTEL.UI.Desktop.ViewModels
{
    public class ErrorsListViewModel : INotifyPropertyChanged
    {
        #region Properties
        public Error SelectedError
        {
            get => _SelectedError;
            set
            {
                _SelectedError = value;
                OnPropertyChanged("SelectedError");
            }
        }

        public string Summary
        {
            get => _summary;
            set
            {
                _summary = value;
                OnPropertyChanged("Summary");
            }
        }
        public ObservableCollection<Error> Errors { get; set; }
        #endregion
        #region Fields
        private Error _SelectedError;

        private string _summary;

        private RelayCommand _selectFirstDataEntryIfExistsCommand;
        #endregion

        public ErrorsListViewModel(ObservableCollection<Error> errorsList)
        {
            Errors = errorsList;
            Summary = string.Format(LocalizationManager.Instance.GetValue("Window.ErrorsList.Summary"), Errors.Count);
        }

        #region Commands
        public RelayCommand SelectFirstDataEntryIfExistsCommand
        {
            get
            {
                return _selectFirstDataEntryIfExistsCommand ??= new RelayCommand(obj =>
                {
                    Error error;
                    try
                    {
                        error = Errors.First();
                    }
                    catch
                    {
                        return;
                    }
                    SelectedError = error;
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
