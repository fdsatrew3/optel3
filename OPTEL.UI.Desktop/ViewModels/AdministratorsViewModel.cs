using OPTEL.Data.Users;
using OPTEL.UI.Desktop.Helpers;
using OPTEL.UI.Desktop.Services.ErrorsListWindows.Base;
using OPTEL.UI.Desktop.Services.WindowClosers.Base;
using OPTEL.UI.Desktop.ViewModels.Core;
using System.Collections.ObjectModel;
using System.Linq;

namespace OPTEL.UI.Desktop.ViewModels
{
    public class AdministratorsViewModel : DatabaseEntityViewModel
    {
        #region Properties
        public Administrator SelectedAdministrator
        {
            get => _SelectedAdministrator;
            set
            {
                _SelectedAdministrator = value;
                OnPropertyChanged("SelectedAdministrator");
                IsDataChanged = false;
            }
        }
        public ObservableCollection<Administrator> Administrators { get; set; }
        #endregion
        #region Fields
        private Administrator _SelectedAdministrator;

        private RelayCommand _selectFirstDataEntryIfExistsCommand;
        private RelayCommand _addEntityCommand;
        private RelayCommand _removeEntityCommand;
        private RelayCommand _cloneEntityCommand;
        #endregion

        public AdministratorsViewModel(IWindowCloseService windowCloseService, IErrorsListWindowService errorsListService) : base(windowCloseService, errorsListService)
        {
            Administrators = new ObservableCollection<Administrator>(Database.instance.AdministratorRepository.GetAll());
        }

        #region Commands
        public RelayCommand AddEntityCommand
        {
            get
            {
                return _addEntityCommand ??= new RelayCommand(obj =>
                {
                    Administrator change = new Administrator();
                    Administrators.Add(change);
                    Database.instance.AdministratorRepository.Add(change);
                    SelectedAdministrator = change;
                });
            }
        }
        public RelayCommand RemoveEntityCommand
        {
            get
            {
                return _removeEntityCommand ??= new RelayCommand(obj =>
                {
                    Database.instance.AdministratorRepository.Delete(SelectedAdministrator);
                    Administrators.Remove(SelectedAdministrator);
                    SelectFirstDataEntryIfExistsCommand.Execute(null);
                }, (obj) => SelectedAdministrator != null);
            }
        }
        public RelayCommand CloneEntityCommand
        {
            get
            {
                return _cloneEntityCommand ??= new RelayCommand(obj =>
                {
                    Administrator change = new Administrator();
                    change.Login = SelectedAdministrator.Login;
                    change.Password = SelectedAdministrator.Password;
                    Administrators.Add(change);
                    Database.instance.AdministratorRepository.Add(change);
                    SelectedAdministrator = change;
                }, (obj) => SelectedAdministrator != null);
            }
        }
        #endregion

        public override void SelectFirstDataEntryIfExist()
        {
            Administrator administrator;
            try
            {
                administrator = Administrators.First();
            }
            catch
            {
                return;
            }
            SelectedAdministrator = administrator;
        }
    }
}
