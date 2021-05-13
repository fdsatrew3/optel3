using Braincase.GanttChart;
using EasyLocalization.Localization;
using OPTEL.Data;
using OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Time;
using OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Time.Base;
using OPTEL.UI.Desktop.Models;
using OPTEL.UI.Desktop.Services.GanttChartManagers.Base;
using OPTEL.UI.Desktop.ViewModels;
using OPTEL.UI.Desktop.Views;
using System;
using System.Diagnostics;

namespace OPTEL.UI.Desktop.Services.GanttChartManagers
{
    public class MainWindowGanttChartManagerService : IGanttChartManagerService
    {
        Chart _ganttChart;
        ProductionPlan _plan;
        ProjectManager _projectManager;
        DateTime _planningStartDate, _planningEndDate;
        IOrderExcecutionTimeCalculator _orderExecutionTimeCalculator;
        IOrdersReconfigurationTimeCalculator _ordersReconfigurationTimeCalculator;
        MainWindowViewModel _viewModel;
        ObjectiveFunction _productionPlanTargetFunction;
        IProductionLineQueueTimeCalculator _productionLineQueueTimeCalculator;

        public MainWindowGanttChartManagerService(MainWindowViewModel viewModel, Chart ganttChart, IOrderExcecutionTimeCalculator orderExecutionTimeCalculator, IOrdersReconfigurationTimeCalculator ordersReconfigurationTimeCalculator, IProductionLineQueueTimeCalculator productionLineQueueTimeCalculator)
        {
            _ganttChart = ganttChart ?? throw new Exception("Chart is null");
            _planningStartDate = DateTime.MinValue;
            _planningEndDate = DateTime.MaxValue;
            _orderExecutionTimeCalculator = orderExecutionTimeCalculator ?? throw new Exception("IOrderExcecutionTimeCalculator is null");
            _ordersReconfigurationTimeCalculator = ordersReconfigurationTimeCalculator ?? throw new Exception("IOrdersReconfigurationTimeCalculator is null");
            _productionLineQueueTimeCalculator = productionLineQueueTimeCalculator ?? throw new Exception("IProductionLineQueueTimeCalculator is null");
            _viewModel = viewModel;
            DisableUserInput(_ganttChart);
        }

        private void DisableUserInput(Chart ganttChart)
        {
            ganttChart.AllowTaskDragDrop = false;
            ganttChart.AllowDrop = false;
        }

        public void SetDesiredInterval(DateTime start, DateTime end)
        {
            _planningStartDate = start;
            _planningEndDate = end;
        }

        public void SetProductionPlan(ProductionPlan plan)
        {

            _plan = plan;
            _projectManager = new ProjectManager();
            TimeSpan timeOffset = TimeSpan.FromDays(0.5);
            _projectManager.Start = _planningStartDate - timeOffset;
            TimeSpan productionPlanExecutionTime = TimeSpan.FromSeconds(0);
            GanttChartTask lastCreatedTask;
            foreach (ProductionLineQueue queue in plan.ProductionLineQueues)
            {
                GanttChartTask productionLineTask = new GanttChartTask(queue.ProductionLine);
                _projectManager.Add(productionLineTask);
                _projectManager.SetStart(productionLineTask, timeOffset);
                _projectManager.SetDuration(productionLineTask, TimeSpan.FromMinutes(_productionLineQueueTimeCalculator.Calculate(queue)));
                productionPlanExecutionTime = productionLineTask.Duration > productionPlanExecutionTime ? productionLineTask.Duration : productionPlanExecutionTime;
                lastCreatedTask = productionLineTask;
                foreach (Order order in queue.Orders)
                {
                    GanttChartTask orderTask = new GanttChartTask(queue.ProductionLine, order);
                    TimeSpan orderDuration = TimeSpan.FromMinutes(_orderExecutionTimeCalculator.Calculate(order));
                    TimeSpan orderStart = lastCreatedTask.Start;
                    if (lastCreatedTask.Order != null)
                    {
                        orderDuration += TimeSpan.FromMinutes(_ordersReconfigurationTimeCalculator.Calculate(queue.ProductionLine, lastCreatedTask.Order, order));
                        orderStart = lastCreatedTask.End;
                    }
                    _projectManager.Add(orderTask);
                    _projectManager.SetStart(orderTask, orderStart);
                    _projectManager.SetDuration(orderTask, orderDuration);
                    _projectManager.Group(productionLineTask, orderTask);
                    _projectManager.Relate(lastCreatedTask, orderTask);
                    lastCreatedTask = orderTask;
                }
            }
            foreach (GanttChartTask task in _projectManager.Tasks)
            {
                _ganttChart.SetToolTip(task, (_projectManager.Start + task.Start) + " - " + (_projectManager.Start + task.Start + task.Duration));
            }
            switch (_productionPlanTargetFunction.Type)
            {
                case ObjectiveFunction.Types.Cost:
                    //_viewModel.TargetFunctionString = string.Format(LocalizationManager.Instance.GetValue("Window.Main.GanttChart.CostTargetFunction"), Math.Round(productionPlanExecutionPrice, 2));
                    break;
                case ObjectiveFunction.Types.Time:
                    _viewModel.TargetFunctionString = string.Format(LocalizationManager.Instance.GetValue("Window.Main.GanttChart.TimeTargetFunction"), productionPlanExecutionTime.ToString("dd"), productionPlanExecutionTime.ToString("hh\\:mm\\:ss"));
                    break;
            }
        }

        public void SetTargetFunction(ObjectiveFunction function)
        {
            _productionPlanTargetFunction = function;
        }

        public void UpdateChart()
        {
            if (_projectManager == null)
            {
                throw new Exception("ProjectManager of GanttChart is null");
            }
            _ganttChart.Init(_projectManager);
            _ganttChart.CreateTaskDelegate = delegate () { return new GanttChartTask(); };
        }

        [Serializable]
        public class GanttChartTask : Task
        {
            public GanttChartTask(ProductionLine productionLine = null, Order order = null) : base()
            {
                ProductionLine = productionLine;
                Order = order;
                string name = string.Empty;
                if (ProductionLine != null)
                {
                    if (Order != null)
                    {
                        name = string.Format("{0} ({1} {2})", order.OrderNumber, productionLine.Name, productionLine.Code);
                    }
                    else
                    {
                        name = string.Format("{0} {1}", productionLine.Name, productionLine.Code);
                    }
                }
                Name = name;
            }
            public Order Order { get; set; }
            public ProductionLine ProductionLine { get; set; }
        }
    }
}
