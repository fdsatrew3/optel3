using EasyLocalization.Localization;
using OPTEL.Data;
using OPTEL.UI.Desktop.Helpers;
using OPTEL.UI.Desktop.Models;
using OPTEL.UI.Desktop.Services.ErrorsListWindows.Base;
using OPTEL.UI.Desktop.Services.WindowClosers.Base;
using System.Collections.ObjectModel;
using System.Linq;

namespace OPTEL.UI.Desktop.ViewModels
{
    public class PlanningConfigViewModel
    {
        #region Properties
        public Customer SelectedCustomer
        {
            get => _SelectedCustomer;
            set
            {
                _SelectedCustomer = value;
                OnPropertyChanged("SelectedCustomer");
                IsDataChanged = false;
            }
        }
        public ObservableCollection<Customer> Customers { get; set; }
        #endregion
        #region Fields
        private Customer _SelectedCustomer;

        private RelayCommand _selectFirstDataEntryIfExistsCommand;
        private RelayCommand _addEntityCommand;
        private RelayCommand _removeEntityCommand;
        private RelayCommand _cloneEntityCommand;
        #endregion

        public PlanningConfigViewModel(IErrorsListWindowService errorsListService)
        {
            Customers = new ObservableCollection<Customer>(Database.instance.CustomerRepository.GetAll());
        }

        #region Commands
        public RelayCommand SelectFirstDataEntryIfExistsCommand
        {
            get
            {
                return _selectFirstDataEntryIfExistsCommand ??= new RelayCommand(obj =>
                {
                    Customer coolingLip;
                    try
                    {
                        coolingLip = Customers.First();
                    }
                    catch
                    {
                        return;
                    }
                    SelectedCustomer = coolingLip;
                });
            }
        }
        public RelayCommand AddEntityCommand
        {
            get
            {
                return _addEntityCommand ??= new RelayCommand(obj =>
                {
                    Customer change = new Customer();
                    Customers.Add(change);
                    Database.instance.CustomerRepository.Add(change);
                    SelectedCustomer = change;
                });
            }
        }
        public RelayCommand RemoveEntityCommand
        {
            get
            {
                return _removeEntityCommand ??= new RelayCommand(obj =>
                {
                    Database.instance.CustomerRepository.Delete(SelectedCustomer);
                    Customers.Remove(SelectedCustomer);
                    SelectFirstDataEntryIfExistsCommand.Execute(null);
                }, (obj) => SelectedCustomer != null);
            }
        }
        public RelayCommand CloneEntityCommand
        {
            get
            {
                return _cloneEntityCommand ??= new RelayCommand(obj =>
                {
                    Customer change = new Customer();
                    change.Name = SelectedCustomer.Name;
                    change.Orders = SelectedCustomer.Orders;
                    Customers.Add(change);
                    Database.instance.CustomerRepository.Add(change);
                    SelectedCustomer = change;
                }, (obj) => SelectedCustomer != null);
            }
        }
        #endregion
        public override ObservableCollection<Error> GetCustomErrors()
        {
            ObservableCollection<Error> errors = new ObservableCollection<Error>();
            int entryIndex = 0;
            for (int i = 0; i < Customers.Count; i++)
            {
                entryIndex = i + 1;
                if (Customers[i].Name == null || Customers[i].Name.Length == 0)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.Customers.Errors.NameIsNull"), entryIndex)
                    });
                }
            }
            return errors;
        }
    }
}
