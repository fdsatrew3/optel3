using EasyLocalization.Localization;
using OPTEL.Data;
using OPTEL.Optimization.Algorithms.Best;
using OPTEL.Optimization.Algorithms.Bruteforce;
using OPTEL.Optimization.Algorithms.FitnessFunctionCalculators;
using OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Cost;
using OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Time;
using OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Time.Base;
using OPTEL.UI.Desktop.Helpers;
using OPTEL.UI.Desktop.Models;
using OPTEL.UI.Desktop.Services.ErrorsListWindows.Base;
using OPTEL.UI.Desktop.Services.GanttChartManagers.Base;
using OPTEL.UI.Desktop.Services.ModelsConverter.Base;
using OPTEL.UI.Desktop.Services.WindowClosers.Base;
using Optimization.Algorithms;
using Optimization.Algorithms.Bruteforce;
using Optimization.Algorithms.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
                OnPropertyChanged("IsBuildDecisionTreeChecked");
            }
        }

        public bool IsBuildingProductionPlan
        {
            get => _isBuildingProductionPlan;
            set
            {
                _isBuildingProductionPlan = value;
                OnPropertyChanged("IsBuildingProductionPlan");
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

        private DataGrid _ordersDataGrid, _productionLinesDataGrid;
        private int _currentSelectedTabIndex, _maxSelectedTabIndex;

        private bool _isBuildDecisionTreeChecked;
        private bool _isBuildingProductionPlan;

        private DateTime? _planningStartDate, _planningEndDate;

        private IErrorsListWindowService _errorsListWindowService;

        private IModelConverterService<PlanningConfigOrder, Order> _planningConfigOrderConverterService;

        private IModelConverterService<PlanningConfigProductionLine, ProductionLine> _planningConfigProductionLineConverterService;

        private IGanttChartManagerService _ganttChartManagerService;

        private IWindowCloseService _windowCloseService;

        private IProductionLineQueueTimeCalculator _productionLineQueueTimeCalculator;

        private RelayCommand _moveToNextTabCommand;
        private RelayCommand _moveToPreviousTabCommand;
        private RelayCommand _startPlanningCommand;
        private RelayCommand _selectAllOrdersCommand;
        private RelayCommand _selectAllProductionLinesCommand;
        private RelayCommand _determineCloseAllowedCommand;
        #endregion

        public PlanningConfigViewModel(IErrorsListWindowService errorsListWindowService, IModelConverterService<PlanningConfigOrder, Order> planningConfigOrderConverterService, IModelConverterService<PlanningConfigProductionLine, ProductionLine> planningConfigProductionLineConverterService, IGanttChartManagerService ganttChartManagerService, IWindowCloseService windowCloseService, IProductionLineQueueTimeCalculator productionLineQueueTimeCalculator, int maxSelectedTabIndex, DataGrid ordersDataGrid, DataGrid productionLinesDataGrid)
        {
            _errorsListWindowService = errorsListWindowService;
            _planningConfigOrderConverterService = planningConfigOrderConverterService;
            _planningConfigProductionLineConverterService = planningConfigProductionLineConverterService;
            _ganttChartManagerService = ganttChartManagerService;
            _windowCloseService = windowCloseService;
            _productionLineQueueTimeCalculator = productionLineQueueTimeCalculator;
            _ordersDataGrid = ordersDataGrid;
            _productionLinesDataGrid = productionLinesDataGrid;
            _currentSelectedTabIndex = 0;
            _maxSelectedTabIndex = maxSelectedTabIndex;
            PlanningStartDate = DateTime.Now;
            PlanningEndDate = PlanningStartDate;
            _windowCloseService.SetDetermineCloseAllowedCommand(DetermineCloseAllowedCommand);
            ObjectiveFunctions = new ObservableCollection<ObjectiveFunction>
            {
                new ObjectiveFunction
                {
                    Name = LocalizationManager.Instance.GetValue("Window.PlanningConfig.TimeObjectiveFunction"),
                    Type = ObjectiveFunction.Types.Time
                },
                new ObjectiveFunction
                {
                    Name = LocalizationManager.Instance.GetValue("Window.PlanningConfig.CostObjectiveFunction"),
                    Type = ObjectiveFunction.Types.Cost
                }
            };
            SelectedObjectiveFunction = ObjectiveFunctions[0];
            PlanningAlgorithms = new ObservableCollection<PlanningAlgorithm>
            {
                new PlanningAlgorithm
                {
                    Name = LocalizationManager.Instance.GetValue("Window.PlanningConfig.GeneticPlanningAlgorithm"),
                    Type = PlanningAlgorithm.Types.Genetic
                },
                new PlanningAlgorithm
                {
                    Name = LocalizationManager.Instance.GetValue("Window.PlanningConfig.BruteforcePlanningAlgorithm"),
                    Type = PlanningAlgorithm.Types.Bruteforce
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
            IsBuildingProductionPlan = false;
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
        public RelayCommand SelectAllOrdersCommand
        {
            get
            {
                return _selectAllOrdersCommand ??= new RelayCommand(obj =>
                {
                    foreach (PlanningConfigOrder order in Orders)
                    {
                        order.IsSelected = true;
                    }
                    _ordersDataGrid.Items.Refresh();
                });
            }
        }
        public RelayCommand SelectAllProductionLinesCommand
        {
            get
            {
                return _selectAllProductionLinesCommand ??= new RelayCommand(obj =>
                {
                    foreach (PlanningConfigProductionLine productionLine in ProductionLines)
                    {
                        productionLine.IsSelected = true;
                    }
                    _productionLinesDataGrid.Items.Refresh();
                });
            }
        }
        public RelayCommand StartPlanningCommand
        {
            get
            {
                return _startPlanningCommand ??= new RelayCommand(async obj =>
                {
                    IsBuildingProductionPlan = true;
                    ObservableCollection<Error> errors = new ObservableCollection<Error>();
                    ObservableCollection<Order> orders = new ObservableCollection<Order>();
                    ObservableCollection<ProductionLine> productionLines = new ObservableCollection<ProductionLine>();
                    foreach (PlanningConfigOrder order in Orders)
                    {
                        if (order.IsSelected == true)
                        {
                            orders.Add(_planningConfigOrderConverterService.ConvertBack(order));
                        }
                    }
                    foreach (PlanningConfigProductionLine productionLine in ProductionLines)
                    {
                        if (productionLine.IsSelected == true)
                        {
                            productionLines.Add(_planningConfigProductionLineConverterService.ConvertBack(productionLine));
                        }
                    }
                    if (PlanningStartDate == null)
                    {
                        errors.Add(new Error
                        {
                            Content = LocalizationManager.Instance.GetValue("Window.PlanningConfig.Errors.PlanningStartDateIsNull")
                        });
                    }
                    if (PlanningEndDate == null)
                    {
                        errors.Add(new Error
                        {
                            Content = LocalizationManager.Instance.GetValue("Window.PlanningConfig.Errors.PlanningEndDateIsNull")
                        });
                    }
                    if (PlanningStartDate != null && PlanningEndDate != null && PlanningStartDate > PlanningEndDate)
                    {
                        errors.Add(new Error
                        {
                            Content = LocalizationManager.Instance.GetValue("Window.PlanningConfig.Errors.PlanningStartDateIsGreaterThanPlanningEndDate")
                        });
                    }
                    if (orders.Count < 3)
                    {
                        errors.Add(new Error
                        {
                            Content = LocalizationManager.Instance.GetValue("Window.PlanningConfig.Errors.OrdersListIsNotEnough")
                        });
                    }
                    if (productionLines.Count == 0)
                    {
                        errors.Add(new Error
                        {
                            Content = LocalizationManager.Instance.GetValue("Window.PlanningConfig.Errors.ProductionLinesListIsEmpty")
                        });
                    }
                    if (SelectedPlanningAlgorithm == null)
                    {
                        errors.Add(new Error
                        {
                            Content = LocalizationManager.Instance.GetValue("Window.PlanningConfig.Errors.PlanningAlgorithmIsNull")
                        });
                    }
                    if (SelectedObjectiveFunction == null)
                    {
                        errors.Add(new Error
                        {
                            Content = LocalizationManager.Instance.GetValue("Window.PlanningConfig.Errors.ObjectiveFunctionIsNull")
                        });
                    }
                    var filmChanges = Database.instance.FilmRecipeChangeRepository.GetAll();
                    IOptimizationAlgorithm<ProductionPlan> planningAlgorithm = null;
                    ProductionPlan optimalProductionPlan = null;
                    try
                    {
                        switch (SelectedPlanningAlgorithm.Type)
                        {
                            case PlanningAlgorithm.Types.Bruteforce:
                                var orderBruteforceAlgorithm = new OrderBruteforceAlgorithm();
                                ITargetFunctionCalculator<ProductionPlan> targetFunctionCalculator = null;
                                switch (SelectedObjectiveFunction.Type)
                                {
                                    case ObjectiveFunction.Types.Cost:
                                        var productionLineQueueCostCalculator = new ProductionLineQueueCostCalculator(_productionLineQueueTimeCalculator);
                                        targetFunctionCalculator = new CostFunctionCalculator(productionLineQueueCostCalculator);
                                        break;
                                    case ObjectiveFunction.Types.Time:
                                        targetFunctionCalculator = new TimeFunctionCalculator(_productionLineQueueTimeCalculator);
                                        break;
                                }
                                var fitnessCalculator = new MinFitnessCalculator(targetFunctionCalculator);
                                planningAlgorithm = new BruteforceAlgorithm(orderBruteforceAlgorithm, orders, productionLines, fitnessCalculator);
                                break;
                            case PlanningAlgorithm.Types.Genetic:
                                planningAlgorithm = new BestAlgorithm(productionLines, orders, filmChanges);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        errors.Add(new Error
                        {
                            Content = ex.Message
                        });
                    }
                    if (planningAlgorithm != null)
                    {
                        await Task.Run(() =>
                        {
                            try
                            {
                                optimalProductionPlan = planningAlgorithm.GetResolve();
                            }
                            catch (Exception ex)
                            {
                                errors.Add(new Error
                                {
                                    Content = ex.Message
                                });
                            }
                        });
                    }
                    else
                    {
                        errors.Add(new Error
                        {
                            Content = "planningAlgorithm is null?"
                        });
                    }
                    if (optimalProductionPlan == null)
                    {
                        errors.Add(new Error
                        {
                            Content = "optimalProductionPlan is null?"
                        });
                    }
                    if (errors.Count == 0)
                    {
                        double executionTime;
                        bool isProductionPlanFitPlanningInterval = true;
                        foreach (ProductionLineQueue queue in optimalProductionPlan.ProductionLineQueues)
                        {
                            executionTime = _productionLineQueueTimeCalculator.Calculate(queue);
                            if (PlanningStartDate + TimeSpan.FromMinutes(executionTime) > PlanningEndDate)
                            {
                                isProductionPlanFitPlanningInterval = false;
                                break;
                            }
                        }
                        bool displayOptimalProductionPlan = true;
                        if (!isProductionPlanFitPlanningInterval)
                        {
                            MessageBoxResult result = MessageBox.Show(
                                LocalizationManager.Instance.GetValue("Window.PlanningConfig.Warnings.ProductionPlanNotFitPlanningInterval"),
                                LocalizationManager.Instance.GetValue("Window.Global.MessageBox.Warning.Global.Title"),
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Warning);
                            if (result == MessageBoxResult.No)
                            {
                                displayOptimalProductionPlan = false;
                            }
                        }
                        if (displayOptimalProductionPlan == true)
                        {
                            try
                            {
                                _ganttChartManagerService.SetDesiredInterval((DateTime)PlanningStartDate, (DateTime)PlanningEndDate);
                                _ganttChartManagerService.SetProductionPlan(optimalProductionPlan);
                                _ganttChartManagerService.UpdateChart();
                            }
                            catch (Exception ex)
                            {
                                errors.Add(new Error
                                {
                                    Content = ex.Message
                                });
                                _errorsListWindowService.SetErrorsForDisplay(errors);
                                _errorsListWindowService.ShowErrorsListWindow();
                            }
                        }
                    }
                    else
                    {
                        _errorsListWindowService.SetErrorsForDisplay(errors);
                        _errorsListWindowService.ShowErrorsListWindow();
                    }
                    IsBuildingProductionPlan = false;
                });
            }
        }

        public RelayCommand DetermineCloseAllowedCommand
        {
            get
            {
                return _determineCloseAllowedCommand ??= new RelayCommand(obj =>
                {
                    if (IsBuildingProductionPlan == true)
                    {
                        _windowCloseService.SetAllowWindowClosing(false);
                        _windowCloseService.SetReasonMessage("Window.PlanningConfig.CalculationInProcess");
                    }
                    else
                    {
                        _windowCloseService.SetAllowWindowClosing(true);
                    }
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
