using EasyLocalization.Localization;
using OPTEL.Data;
using OPTEL.UI.Desktop.Helpers;
using OPTEL.UI.Desktop.Models;
using OPTEL.UI.Desktop.Services.ErrorsListWindows.Base;
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
                IgnoreMarkDataChangedRequestsCommand.Execute(null);
                OnPropertyChanged("SelectedOrder");
                AcceptMarkDataChangedRequestsCommand.Execute(null);
            }
        }
        public ObservableCollection<Order> Orders { get; set; }
        public IEnumerable<Customer> Customers { get; set; }
        public IEnumerable<FilmRecipe> FilmRecipes { get; set; }
        #endregion
        #region Fields
        private Order _SelectedOrder;
        #endregion

        public OrdersViewModel(IWindowCloseService windowCloseService, IErrorsListWindowService errorsListService) : base(windowCloseService, errorsListService)
        {
            Orders = new ObservableCollection<Order>(Database.instance.OrderRepository.GetAll());
            FilmRecipes = Database.instance.FilmRecipeRepository.GetAll();
            Customers = Database.instance.CustomerRepository.GetAll();
        }

        public override ObservableCollection<Error> GetCustomErrors()
        {
            ObservableCollection<Error> errors = new ObservableCollection<Error>();
            int entryIndex = 0;
            for (int i = 0; i < Orders.Count; i++)
            {
                entryIndex = i + 1;
                if (Orders[i].FilmRecipe == null)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.Orders.Errors.FilmRecipeIsNull"), entryIndex)
                    });
                }
                if (Orders[i].OrderNumber == null || Orders[i].OrderNumber.Length == 0)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.Orders.Errors.OrderNumberIsNull"), entryIndex)
                    });
                }
                if (Orders[i].ParentCustomer == null)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.Orders.Errors.ParentCustomerIsNull"), entryIndex)
                    });
                }
                if (Orders[i].PlanningEndDate == null)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.Orders.Errors.PlanningEndDateIsNull"), entryIndex)
                    });
                }
                if (Orders[i].PredefinedTime != null)
                {
                    if (Math.Sign((double)Orders[i].PredefinedTime) < 0)
                    {
                        errors.Add(new Error
                        {
                            Content = string.Format(LocalizationManager.Instance.GetValue("Window.Orders.Errors.PredefinedTimeIsNegative"), entryIndex)
                        });
                    }
                }
                if (Math.Sign(Orders[i].PriceOverdue) < 0)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.Orders.Errors.PriceOverdueIsNegative"), entryIndex)
                    });
                }
                if (Math.Sign(Orders[i].QuantityInRunningMeter) < 0)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.Orders.Errors.QuantityInRunningMeterIsNegative"), entryIndex)
                    });
                }
                if (Math.Sign(Orders[i].Width) < 0)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.Orders.Errors.WidthIsNegative"), entryIndex)
                    });
                }
            }
            return errors;
        }

        public override void SelectFirstDataEntryIfExist()
        {
            Order order;
            try
            {
                order = Orders.First();
            }
            catch
            {
                return;
            }
            SelectedOrder = order;
        }

        public override void AddEntity()
        {
            Order order = new Order();
            order.PlanningEndDate = DateTime.Now;
            Orders.Add(order);
            Database.instance.OrderRepository.Add(order);
            SelectedOrder = order;
        }

        public override void RemoveEntity()
        {
            Database.instance.OrderRepository.Delete(SelectedOrder);
            Orders.Remove(SelectedOrder);
            SelectFirstDataEntryIfExistsCommand.Execute(null);
        }

        public override bool RemoveEntityExecuteCondition()
        {
            return SelectedOrder != null;
        }

        public override void CloneEntity()
        {
            Order order = new Order();
            order.FilmRecipe = SelectedOrder.FilmRecipe;
            order.FilmRecipeID = SelectedOrder.FilmRecipeID;
            order.OrderNumber = SelectedOrder.OrderNumber;
            order.ParentCustomer = SelectedOrder.ParentCustomer;
            order.ParentCustomerID = SelectedOrder.ParentCustomerID;
            order.PlanningEndDate = SelectedOrder.PlanningEndDate;
            order.PredefinedTime = SelectedOrder.PredefinedTime;
            order.PriceOverdue = SelectedOrder.PriceOverdue;
            order.QuantityInRunningMeter = SelectedOrder.QuantityInRunningMeter;
            order.Width = SelectedOrder.Width;
            Orders.Add(order);
            Database.instance.OrderRepository.Add(order);
            SelectedOrder = order;
        }

        public override bool CloneEntityExecuteCondition()
        {
            return SelectedOrder != null;
        }
    }
}
