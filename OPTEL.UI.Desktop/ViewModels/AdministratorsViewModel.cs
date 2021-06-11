using OPTEL.Data.Users;
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
        #endregion

        public AdministratorsViewModel(IWindowCloseService windowCloseService, IErrorsListWindowService errorsListService) : base(windowCloseService, errorsListService)
        {
            Administrators = new ObservableCollection<Administrator>(Database.instance.AdministratorRepository.GetAll());
        }

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
        public override void AddEntity()
        {
            Administrator change = new Administrator();
            Administrators.Add(change);
            Database.instance.AdministratorRepository.Add(change);
            SelectedAdministrator = change;
        }

        public override void RemoveEntity()
        {
            Database.instance.AdministratorRepository.Delete(SelectedAdministrator);
            Administrators.Remove(SelectedAdministrator);
            SelectFirstDataEntryIfExistsCommand.Execute(null);
        }

        public override bool RemoveEntityExecuteCondition()
        {
            return SelectedAdministrator != null;
        }

        public override void CloneEntity()
        {
            Administrator adminstrator = new Administrator();
            adminstrator.Login = SelectedAdministrator.Login;
            adminstrator.Password = SelectedAdministrator.Password;
            Administrators.Add(adminstrator);
            Database.instance.AdministratorRepository.Add(adminstrator);
            SelectedAdministrator = adminstrator;
        }

        public override bool CloneEntityExecuteCondition()
        {
            return SelectedAdministrator != null;
        }
    }
}
