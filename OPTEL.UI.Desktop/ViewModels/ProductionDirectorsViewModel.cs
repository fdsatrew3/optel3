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

        private RelayCommand _addEntityCommand;
        private RelayCommand _removeEntityCommand;
        private RelayCommand _cloneEntityCommand;
        #endregion

        public ProductionDirectorsViewModel(IWindowCloseService windowCloseService, IErrorsListWindowService errorsListService) : base(windowCloseService, errorsListService)
        {
            ProductionDirectors = new ObservableCollection<ProductionDirector>(Database.instance.ProductionDirectorRepository.GetAll());
        }

        #region Commands
        public RelayCommand AddEntityCommand
        {
            get
            {
                return _addEntityCommand ??= new RelayCommand(obj =>
                {
                    ProductionDirector change = new ProductionDirector();
                    ProductionDirectors.Add(change);
                    Database.instance.ProductionDirectorRepository.Add(change);
                    SelectedProductionDirector = change;
                });
            }
        }
        public RelayCommand RemoveEntityCommand
        {
            get
            {
                return _removeEntityCommand ??= new RelayCommand(obj =>
                {
                    Database.instance.ProductionDirectorRepository.Delete(SelectedProductionDirector);
                    ProductionDirectors.Remove(SelectedProductionDirector);
                    SelectFirstDataEntryIfExistsCommand.Execute(null);
                }, (obj) => SelectedProductionDirector != null);
            }
        }
        public RelayCommand CloneEntityCommand
        {
            get
            {
                return _cloneEntityCommand ??= new RelayCommand(obj =>
                {
                    ProductionDirector change = new ProductionDirector();
                    change.Login = SelectedProductionDirector.Login;
                    change.Password = SelectedProductionDirector.Password;
                    ProductionDirectors.Add(change);
                    Database.instance.ProductionDirectorRepository.Add(change);
                    SelectedProductionDirector = change;
                }, (obj) => SelectedProductionDirector != null);
            }
        }
        #endregion

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
    }
}
