using OPTEL.Data;
using OPTEL.UI.Desktop.Helpers;
using OPTEL.UI.Desktop.Models;
using OPTEL.UI.Desktop.Services.ErrorsListWindows.Base;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace OPTEL.UI.Desktop.ViewModels
{
    public class PlanningConfigViewModel : INotifyPropertyChanged
    {
        #region Properties
        public Order SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                _selectedOrder = value;
                OnPropertyChanged("SelectedOrder");
            }
        }

        public ObjectiveFunction SelectedObjectiveFunction
        {
            get => _selectedObjectiveFunction;
            set
            {
                _selectedObjectiveFunction = value;
                OnPropertyChanged("SelectedObjectiveFunction");
            }
        }

        public int CurrentSelectedTabIndex
        {
            get => _currentSelectedTabIndex;
            set
            {
                _currentSelectedTabIndex = value;
                OnPropertyChanged("CurrentSelectedTabIndex");
            }
        }

        public DateTime? PlanningStartDate
        {
            get => _planningStartDate;
            set
            {
                _planningStartDate = value;
                OnPropertyChanged("PlanningStartDate");
            }
        }

        public DateTime? PlanningEndDate
        {
            get => _planningEndDate;
            set
            {
                _planningEndDate = value;
                OnPropertyChanged("PlanningEndDate");
            }
        }
        public ObservableCollection<Order> Orders { get; set; }
        public ObservableCollection<ObjectiveFunction> ObjectiveFunctions { get; set; }
        #endregion
        #region Fields
        private Order _selectedOrder;

        private ObjectiveFunction _selectedObjectiveFunction;

        private int _currentSelectedTabIndex, _maxSelectedTabIndex;

        private DateTime? _planningStartDate, _planningEndDate;

        private IErrorsListWindowService _errorsListWindowService;

        private RelayCommand _moveToNextTabCommand;
        private RelayCommand _moveToPreviousTabCommand;
        private RelayCommand _startPlanningCommand;
        #endregion

        public PlanningConfigViewModel(IErrorsListWindowService errorsListWindowService, int maxSelectedTabIndex)
        {
            Orders = new ObservableCollection<Order>(Database.instance.OrderRepository.GetAll());
            _errorsListWindowService = errorsListWindowService;
            _currentSelectedTabIndex = 0;
            _maxSelectedTabIndex = maxSelectedTabIndex;
            PlanningStartDate = DateTime.Now;
            PlanningEndDate = PlanningStartDate;
            ObjectiveFunctions = new ObservableCollection<ObjectiveFunction>
            {
                new ObjectiveFunction
                {
                    Name = "Time",
                },
                new ObjectiveFunction
                {
                    Name = "Cost"
                }
            };
            SelectedObjectiveFunction = ObjectiveFunctions[0];
        }

        #region Commands
        public RelayCommand MoveToNextTabCommand
        {
            get
            {
                return _moveToNextTabCommand ??= new RelayCommand(obj =>
                {
                    CurrentSelectedTabIndex = Math.Min(CurrentSelectedTabIndex + 1, _maxSelectedTabIndex);
                });
            }
        }
        public RelayCommand MoveToPreviousTabCommand
        {
            get
            {
                return _moveToPreviousTabCommand ??= new RelayCommand(obj =>
                {
                    CurrentSelectedTabIndex = Math.Max(CurrentSelectedTabIndex - 1, 0);
                });
            }
        }
        public RelayCommand StartPlanningCommand
        {
            get
            {
                return _startPlanningCommand ??= new RelayCommand(obj =>
                {

                });
            }
        }
        #endregion

        #region PropertyChangedEventHandler
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
