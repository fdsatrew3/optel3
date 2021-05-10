using OPTEL.Data;
using OPTEL.UI.Desktop.Helpers;
using OPTEL.UI.Desktop.Models;
using OPTEL.UI.Desktop.Services.ErrorsListWindows.Base;
using OPTEL.UI.Desktop.Services.GanttChartManagers.Base;
using OPTEL.UI.Desktop.Services.ModelsConverter.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace OPTEL.UI.Desktop.ViewModels
{
    public class PlanningConfigViewModel : INotifyPropertyChanged
    {
        #region Properties
        public PlanningConfigOrder SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                _selectedOrder = value;
                OnPropertyChanged("SelectedOrder");
            }
        }

        public PlanningConfigProductionLine SelectedProductionLine
        {
            get => _selectedProductionLine;
            set
            {
                _selectedProductionLine = value;
                OnPropertyChanged("SelectedProductionLine");
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

        public PlanningAlgorithm SelectedPlanningAlgorithm
        {
            get => _selectedPlanningAlgorithm;
            set
            {
                _selectedPlanningAlgorithm = value;
                OnPropertyChanged("SelectedPlanningAlgorithm");
            }
        }
        public bool IsBuildDecisionTreeChecked
        {
            get => _isBuildDecisionTreeChecked;
            set
            {
                _isBuildDecisionTreeChecked = value;
                OnPropertyChanged("BuildDecisionTree");
            }
        }
        public ObservableCollection<PlanningAlgorithm> PlanningAlgorithms { get; set; }

        public ObservableCollection<PlanningConfigOrder> Orders { get; set; }
        public ObservableCollection<PlanningConfigProductionLine> ProductionLines { get; set; }
        public ObservableCollection<ObjectiveFunction> ObjectiveFunctions { get; set; }
        #endregion
        #region Fields
        private PlanningConfigOrder _selectedOrder;

        private PlanningConfigProductionLine _selectedProductionLine;

        private PlanningAlgorithm _selectedPlanningAlgorithm;

        private ObjectiveFunction _selectedObjectiveFunction;

        private int _currentSelectedTabIndex, _maxSelectedTabIndex;

        private bool _isBuildDecisionTreeChecked;

        private DateTime? _planningStartDate, _planningEndDate;

        private IErrorsListWindowService _errorsListWindowService;

        private IModelConverterService<PlanningConfigOrder, Order> _planningConfigOrderConverterService;

        private IModelConverterService<PlanningConfigProductionLine, ProductionLine> _planningConfigProductionLineConverterService;

        private IGanttChartManagerService _ganttChartManagerService;

        private RelayCommand _moveToNextTabCommand;
        private RelayCommand _moveToPreviousTabCommand;
        private RelayCommand _startPlanningCommand;
        #endregion

        public PlanningConfigViewModel(IErrorsListWindowService errorsListWindowService, IModelConverterService<PlanningConfigOrder, Order> planningConfigOrderConverterService, IModelConverterService<PlanningConfigProductionLine, ProductionLine> planningConfigProductionLineConverterService, IGanttChartManagerService ganttChartManagerService, int maxSelectedTabIndex)
        {
            _errorsListWindowService = errorsListWindowService;
            _planningConfigOrderConverterService = planningConfigOrderConverterService;
            _planningConfigProductionLineConverterService = planningConfigProductionLineConverterService;
            _ganttChartManagerService = ganttChartManagerService;
            _currentSelectedTabIndex = 0;
            _maxSelectedTabIndex = maxSelectedTabIndex;
            PlanningStartDate = DateTime.Now;
            PlanningEndDate = PlanningStartDate;
            ObjectiveFunctions = new ObservableCollection<ObjectiveFunction>
            {
                new ObjectiveFunction
                {
                    Name = "Time",
                    Type = ObjectiveFunction.Types.Time
                },
                new ObjectiveFunction
                {
                    Name = "Cost",
                    Type = ObjectiveFunction.Types.Cost
                }
            };
            SelectedObjectiveFunction = ObjectiveFunctions[0];
            PlanningAlgorithms = new ObservableCollection<PlanningAlgorithm>
            {
                new PlanningAlgorithm
                {
                    Name = "Genetic",
                    Type = PlanningAlgorithm.Types.Genetic
                },
                new PlanningAlgorithm
                {
                    Name = "Bruteforce",
                    Type = PlanningAlgorithm.Types.Bruteforce
                },
                new PlanningAlgorithm
                {
                    Name = "Best",
                    Type = PlanningAlgorithm.Types.Best
                }
            };
            SelectedPlanningAlgorithm = PlanningAlgorithms[0];
            Orders = new ObservableCollection<PlanningConfigOrder>();
            IEnumerable<Order> orders = Database.instance.OrderRepository.GetAll();
            foreach (Order order in orders)
            {
                Orders.Add(_planningConfigOrderConverterService.Convert(order));
            }
            ProductionLines = new ObservableCollection<PlanningConfigProductionLine>();
            IEnumerable<ProductionLine> productionLines = Database.instance.ProductionLineRepository.GetAll();
            foreach (ProductionLine productionLine in productionLines)
            {
                ProductionLines.Add(_planningConfigProductionLineConverterService.Convert(productionLine));
            }
            IsBuildDecisionTreeChecked = true;
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
