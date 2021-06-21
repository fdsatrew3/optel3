using OPTEL.Data.Users;
using OPTEL.UI.Desktop.Services.ErrorsListWindows.Base;
using OPTEL.UI.Desktop.Services.ModelsConverter;
using OPTEL.UI.Desktop.Services.WindowClosers.Base;
using OPTEL.UI.Desktop.ViewModels.Core;
using OPTEL.UI.Desktop.Views;
using System.Collections.ObjectModel;
using System.Linq;

namespace OPTEL.UI.Desktop.ViewModels
{
    public class AdministratorsViewModel : DatabaseEntityViewModel
    {
        #region Properties
        public Models.User SelectedAdministrator
        {
            get => _selectedAdministrator;
            set
            {
                _selectedAdministrator = value;
                IgnoreMarkDataChangedRequestsCommand.Execute(null);
                OnPropertyChanged("SelectedAdministrator");
                AcceptMarkDataChangedRequestsCommand.Execute(null);
            }
        }
        public ObservableCollection<Models.User> Administrators { get; set; }
        #endregion
        #region Fields
        private Models.User _selectedAdministrator;

        private UserToDataUserConverterService<Administrator> _userConverterService;
        #endregion
        public AdministratorsViewModel(IWindowCloseService windowCloseService, IErrorsListWindowService errorsListService, UserToDataUserConverterService<Administrator> userConverterService) : base(windowCloseService, errorsListService)
        {
            _userConverterService = userConverterService;
            Administrators = new ObservableCollection<Models.User>();
            var admins = new ObservableCollection<Administrator>(Database.instance.AdministratorRepository.GetAll());
            foreach (var admin in admins)
            {
                var convertedAdmin = _userConverterService.Convert(admin);
                convertedAdmin.IsPasswordEncrypted = true;
                Administrators.Add(convertedAdmin);
            }
        }

        public override void SelectFirstDataEntryIfExist()
        {
            Models.User administrator;
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
            Administrator administrator = new Administrator();
            Models.User convertedAdminstrator = new Models.User(administrator);
            Database.instance.AdministratorRepository.Add(administrator);
            Administrators.Add(convertedAdminstrator);
            SelectedAdministrator = convertedAdminstrator;
        }

        public override void RemoveEntity()
        {
            Database.instance.AdministratorRepository.Delete((Administrator)SelectedAdministrator.DataUser);
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
            adminstrator.Login = SelectedAdministrator.DataUser.Login;
            adminstrator.Password = SelectedAdministrator.DataUser.Password;
            var convertedAdministrator = new Models.User(adminstrator);
            convertedAdministrator.IsPasswordEncrypted = SelectedAdministrator.IsPasswordEncrypted;
            Administrators.Add(convertedAdministrator);
            Database.instance.AdministratorRepository.Add(adminstrator);
            SelectedAdministrator = convertedAdministrator;
        }

        public override bool CloneEntityExecuteCondition()
        {
            return SelectedAdministrator != null;
        }

        public override void OnSaveChanges()
        {
            foreach (var admin in Administrators)
            {
                if (admin.IsPasswordEncrypted == false)
                {
                    admin.DataUser.Password = LoginWindow.Encrypt(admin.DataUser.Password);
                    admin.IsPasswordEncrypted = true;
                }
            }
            OnPropertyChanged("SelectedAdministrator");
        }
    }
}
