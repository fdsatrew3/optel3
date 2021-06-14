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
    public class ProductionDirectorsViewModel : DatabaseEntityViewModel
    {
        #region Properties
        public Models.User SelectedProductionDirector
        {
            get => _selectedProductionDirector;
            set
            {
                _selectedProductionDirector = value;
                IgnoreMarkDataChangedRequestsCommand.Execute(null);
                OnPropertyChanged("SelectedProductionDirector");
                AcceptMarkDataChangedRequestsCommand.Execute(null);
            }
        }
        public ObservableCollection<Models.User> ProductionDirectors { get; set; }
        #endregion
        #region Fields
        private Models.User _selectedProductionDirector;

        private UserToDataUserConverterService<ProductionDirector> _userConverterService;
        #endregion
        public ProductionDirectorsViewModel(IWindowCloseService windowCloseService, IErrorsListWindowService errorsListService, UserToDataUserConverterService<ProductionDirector> userConverterService) : base(windowCloseService, errorsListService)
        {
            _userConverterService = userConverterService;
            ProductionDirectors = new ObservableCollection<Models.User>();
            var productionDirectors = new ObservableCollection<ProductionDirector>(Database.instance.ProductionDirectorRepository.GetAll());
            foreach (var productionDirector in productionDirectors)
            {
                var convertedProductionDirector = _userConverterService.Convert(productionDirector);
                convertedProductionDirector.IsPasswordEncrypted = true;
                ProductionDirectors.Add(convertedProductionDirector);
            }
        }

        public override void SelectFirstDataEntryIfExist()
        {
            Models.User director;
            try
            {
                director = ProductionDirectors.First();
            }
            catch
            {
                return;
            }
            SelectedProductionDirector = director;
        }

        public override void AddEntity()
        {
            ProductionDirector productionDirector = new ProductionDirector();
            Models.User convertedProductionDirector = new Models.User(productionDirector);
            Database.instance.ProductionDirectorRepository.Add(productionDirector);
            ProductionDirectors.Add(convertedProductionDirector);
            SelectedProductionDirector = convertedProductionDirector;
        }

        public override void RemoveEntity()
        {
            Database.instance.ProductionDirectorRepository.Delete((ProductionDirector)SelectedProductionDirector.DataUser);
            ProductionDirectors.Remove(SelectedProductionDirector);
            SelectFirstDataEntryIfExistsCommand.Execute(null);
        }

        public override bool RemoveEntityExecuteCondition()
        {
            return SelectedProductionDirector != null;
        }

        public override void CloneEntity()
        {
            ProductionDirector productionDirector = new ProductionDirector();
            productionDirector.Login = SelectedProductionDirector.DataUser.Login;
            productionDirector.Password = SelectedProductionDirector.DataUser.Password;
            var convertedProductionDirector = new Models.User(productionDirector);
            convertedProductionDirector.IsPasswordEncrypted = SelectedProductionDirector.IsPasswordEncrypted;
            ProductionDirectors.Add(convertedProductionDirector);
            Database.instance.ProductionDirectorRepository.Add(productionDirector);
            SelectedProductionDirector = convertedProductionDirector;
        }

        public override bool CloneEntityExecuteCondition()
        {
            return SelectedProductionDirector != null;
        }
        public override void OnSaveChanges()
        {
            foreach (var productionDirector in ProductionDirectors)
            {
                if (productionDirector.IsPasswordEncrypted == false)
                {
                    productionDirector.DataUser.Password = LoginWindow.Encrypt(productionDirector.DataUser.Password);
                    productionDirector.IsPasswordEncrypted = true;
                }
            }
            OnPropertyChanged("SelectedProductionDirector");
        }
    }
}
