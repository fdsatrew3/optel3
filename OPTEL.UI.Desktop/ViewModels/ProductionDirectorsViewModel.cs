using OPTEL.Data.Users;
using OPTEL.UI.Desktop.Helpers;
using OPTEL.UI.Desktop.Services.ErrorsListWindows.Base;
using OPTEL.UI.Desktop.Services.WindowClosers.Base;
using OPTEL.UI.Desktop.ViewModels.Core;
using System.Collections.ObjectModel;
using System.Linq;

namespace OPTEL.UI.Desktop.ViewModels
{
    public class ProductionDirectorsViewModel : DatabaseEntityViewModel
    {
        #region Properties
        public ProductionDirector SelectedProductionDirector
        {
            get => _SelectedProductionDirector;
            set
            {
                _SelectedProductionDirector = value;
                OnPropertyChanged("SelectedProductionDirector");
                IsDataChanged = false;
            }
        }
        public ObservableCollection<ProductionDirector> ProductionDirectors { get; set; }
        #endregion
        #region Fields
        private ProductionDirector _SelectedProductionDirector;
        #endregion

        public ProductionDirectorsViewModel(IWindowCloseService windowCloseService, IErrorsListWindowService errorsListService) : base(windowCloseService, errorsListService)
        {
            ProductionDirectors = new ObservableCollection<ProductionDirector>(Database.instance.ProductionDirectorRepository.GetAll());
        }

        public override void SelectFirstDataEntryIfExist()
        {
            ProductionDirector director;
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
            ProductionDirectors.Add(productionDirector);
            Database.instance.ProductionDirectorRepository.Add(productionDirector);
            SelectedProductionDirector = productionDirector;
        }

        public override void RemoveEntity()
        {
            Database.instance.ProductionDirectorRepository.Delete(SelectedProductionDirector);
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
            productionDirector.Login = SelectedProductionDirector.Login;
            productionDirector.Password = SelectedProductionDirector.Password;
            ProductionDirectors.Add(productionDirector);
            Database.instance.ProductionDirectorRepository.Add(productionDirector);
            SelectedProductionDirector = productionDirector;
        }

        public override bool CloneEntityExecuteCondition()
        {
            return SelectedProductionDirector != null;
        }
    }
}
