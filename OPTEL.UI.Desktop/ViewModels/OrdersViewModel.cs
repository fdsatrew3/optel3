using EasyLocalization.Localization;
using OPTEL.Data;
using OPTEL.UI.Desktop.Helpers;
using OPTEL.UI.Desktop.Services.WindowClosers.Base;
using OPTEL.UI.Desktop.ViewModels.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace OPTEL.UI.Desktop.ViewModels
{
    public class OrdersViewModel : DatabaseEntityViewModel
    {
        #region Properties
        public Order SelectedOrder
        {
            get => _SelectedOrder;
            set
            {
                _SelectedOrder = value;
                OnPropertyChanged("SelectedOrder");
            }
        }
        public ObservableCollection<Order> Orders { get; set; }
        public IEnumerable<Customer> Customers { get; set; }
        public IEnumerable<FilmRecipe> FilmRecipes { get; set; }
        #endregion
        #region Fields
        private Order _SelectedOrder;

        private RelayCommand _selectFirstDataEntryIfExistsCommand;
        private RelayCommand _addEntityCommand;
        private RelayCommand _removeEntityCommand;
        private RelayCommand _cloneEntityCommand;
        #endregion

        public OrdersViewModel(IWindowCloseService windowCloseService) : base(windowCloseService)
        {
            Orders = new ObservableCollection<Order>(Database.instance.OrderRepository.GetAll());
            FilmRecipes = Database.instance.FilmRecipeRepository.GetAll();
            Customers = Database.instance.CustomerRepository.GetAll();
        }

        #region Commands
        public RelayCommand SelectFirstDataEntryIfExistsCommand
        {
            get
            {
                return _selectFirstDataEntryIfExistsCommand ??= new RelayCommand(obj =>
                {
                    Order coolingLip;
                    try
                    {
                        coolingLip = Orders.First();
                    }
                    catch
                    {
                        return;
                    }
                    SelectedOrder = coolingLip;
                });
            }
        }
        public RelayCommand AddEntityCommand
        {
            get
            {
                return _addEntityCommand ??= new RelayCommand(obj =>
                {
                    Order change = new Order();
                    change.PlanningEndDate = DateTime.Now;
                    Orders.Add(change);
                    Database.instance.OrderRepository.Add(change);
                    SelectedOrder = change;
                });
            }
        }
        public RelayCommand RemoveEntityCommand
        {
            get
            {
                return _removeEntityCommand ??= new RelayCommand(obj =>
                {
                    Database.instance.OrderRepository.Delete(SelectedOrder);
                    Orders.Remove(SelectedOrder);
                    SelectFirstDataEntryIfExistsCommand.Execute(null);
                }, (obj) => SelectedOrder != null);
            }
        }
        public RelayCommand CloneEntityCommand
        {
            get
            {
                return _cloneEntityCommand ??= new RelayCommand(obj =>
                {
                    Order change = new Order();
                    change.FilmRecipe = SelectedOrder.FilmRecipe;
                    change.FilmRecipeID = SelectedOrder.FilmRecipeID;
                    change.OrderNumber = SelectedOrder.OrderNumber;
                    change.ParentCustomer = SelectedOrder.ParentCustomer;
                    change.ParentCustomerID = SelectedOrder.ParentCustomerID;
                    change.PlanningEndDate = SelectedOrder.PlanningEndDate;
                    change.PredefinedTime = SelectedOrder.PredefinedTime;
                    change.PriceOverdue = SelectedOrder.PriceOverdue;
                    change.QuantityInRunningMeter = SelectedOrder.QuantityInRunningMeter;
                    change.Width = SelectedOrder.Width;
                    Orders.Add(change);
                    Database.instance.OrderRepository.Add(change);
                    SelectedOrder = change;
                }, (obj) => SelectedOrder != null);
            }
        }
        #endregion
        public override string GetCustomErrorString()
        {
            string result = string.Empty;
            StringBuilder sb = new StringBuilder(result);
            for (int i = 0; i < Orders.Count; i++)
            {
                if (Orders[i].FilmRecipe == null)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.Orders.Errors.FilmRecipeIsNull"), i));
                }
                if (Orders[i].OrderNumber == null || Orders[i].OrderNumber.Length == 0)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.Orders.Errors.OrderNumberIsNull"), i));
                }
                if (Orders[i].ParentCustomer == null)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.Orders.Errors.ParentCustomerIsNull"), i));
                }
                if (Orders[i].PlanningEndDate == null)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.Orders.Errors.PlanningEndDateIsNull"), i));
                }
                if (Math.Sign(Orders[i].PredefinedTime) < 0)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.Orders.Errors.PredefinedTimeIsNegative"), i));
                }
                if (Math.Sign(Orders[i].PriceOverdue) < 0)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.Orders.Errors.PriceOverdueIsNegative"), i));
                }
                if (Math.Sign(Orders[i].QuantityInRunningMeter) < 0)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.Orders.Errors.QuantityInRunningMeterIsNegative"), i));
                }
                if (Math.Sign(Orders[i].Width) < 0)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.Orders.Errors.WidthIsNegative"), i));
                }
            }
            return sb.ToString();
        }
    }
}
