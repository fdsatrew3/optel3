using OPTEL.Data;
using OPTEL.UI.Desktop.Helpers;
using OPTEL.UI.Desktop.ViewModels.Core;
using System.Collections.ObjectModel;
using System.Linq;

namespace OPTEL.UI.Desktop.ViewModels
{
    public class CustomersViewModel : DatabaseEntityViewModel
    {
        #region Properties
        public Customer SelectedCustomer
        {
            get => _SelectedCustomer;
            set
            {
                _SelectedCustomer = value;
                OnPropertyChanged("SelectedCustomer");
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

        public CustomersViewModel()
        {
            Customers = new ObservableCollection<Customer>(Database.instance.CustomerRepository.GetAll());
        }
    }
}
