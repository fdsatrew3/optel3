using EasyLocalization.Localization;
using OPTEL.Data;
using OPTEL.UI.Desktop.Models;
using OPTEL.UI.Desktop.Services.ErrorsListWindows.Base;
using OPTEL.UI.Desktop.Services.WindowClosers.Base;
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
                IsDataChanged = false;
            }
        }
        public ObservableCollection<Customer> Customers { get; set; }
        #endregion
        #region Fields
        private Customer _SelectedCustomer;
        #endregion

        public CustomersViewModel(IWindowCloseService windowCloseService, IErrorsListWindowService errorsListService) : base(windowCloseService, errorsListService)
        {
            Customers = new ObservableCollection<Customer>(Database.instance.CustomerRepository.GetAll());
        }

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

        public override void SelectFirstDataEntryIfExist()
        {
            Customer customer;
            try
            {
                customer = Customers.First();
            }
            catch
            {
                return;
            }
            SelectedCustomer = customer;
        }

        public override void AddEntity()
        {
            Customer customer = new Customer();
            Customers.Add(customer);
            Database.instance.CustomerRepository.Add(customer);
            SelectedCustomer = customer;
        }

        public override void RemoveEntity()
        {
            Database.instance.CustomerRepository.Delete(SelectedCustomer);
            Customers.Remove(SelectedCustomer);
            SelectFirstDataEntryIfExistsCommand.Execute(null);
        }

        public override bool RemoveEntityExecuteCondition()
        {
            return SelectedCustomer != null;
        }

        public override void CloneEntity()
        {
            Customer customer = new Customer();
            customer.Name = SelectedCustomer.Name;
            customer.Orders = SelectedCustomer.Orders;
            Customers.Add(customer);
            Database.instance.CustomerRepository.Add(customer);
            SelectedCustomer = customer;
        }

        public override bool CloneEntityExecuteCondition()
        {
            return SelectedCustomer != null;
        }
    }
}
