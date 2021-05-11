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
            _ordersReconfigurationTimeCalculator = ordersReconfigurationTimeCalculator;
            _productionLineQueueTimeCalculator = productionLineQueueTimeCalculator;
            _viewModel = viewModel;
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
            _projectManager.Start = _planningStartDate;
            TimeSpan productionPlanDuration = _planningEndDate - _planningStartDate;
            TimeSpan currentTimeSpanOffset;
            TimeSpan currentOrderExecutionTime, currentOrderReconfigurationTime;
            Task lastOrderTask;
            Order lastOrder;
            TimeSpan productionPlanExecutionTime = TimeSpan.FromSeconds(0);
            double productionPlanExecutionPrice = 0;
            foreach (ProductionLineQueue queue in plan.ProductionLineQueues)
            {
                Task productionLineTask = new Task();
                productionLineTask.Name = queue.ProductionLine.Name;
                _projectManager.Add(productionLineTask);
                _projectManager.SetStart(productionLineTask, TimeSpan.FromHours(0));
                TimeSpan currentQueueExecutionTime = TimeSpan.FromMinutes(_productionLineQueueTimeCalculator.Calculate(queue));
                if (currentQueueExecutionTime > productionPlanExecutionTime)
                {
                    productionPlanExecutionTime = currentQueueExecutionTime;
                }
                productionPlanExecutionPrice += currentQueueExecutionTime.TotalHours * queue.ProductionLine.HourCost;
                _projectManager.SetDuration(productionLineTask, currentQueueExecutionTime);
                //_ganttChart.SetToolTip(productionLineTask, "Tooltip");
                lastOrderTask = productionLineTask;
                currentTimeSpanOffset = TimeSpan.FromSeconds(0);
                lastOrder = null;
                foreach (Order order in queue.Orders)
                {
                    if (lastOrder == null)
                    {
                        lastOrder = order;
                    }
                    Task orderTask = new Task();
                    orderTask.Name = order.OrderNumber;
                    currentOrderExecutionTime = TimeSpan.FromMinutes(_orderExecutionTimeCalculator.Calculate(order));
                    currentOrderReconfigurationTime = TimeSpan.FromMinutes(_ordersReconfigurationTimeCalculator.Calculate(queue.ProductionLine, lastOrder, order));

                    currentTimeSpanOffset += currentOrderExecutionTime + currentOrderReconfigurationTime;
                    _projectManager.Add(orderTask);
                    _projectManager.SetStart(orderTask, currentTimeSpanOffset);
                    _projectManager.SetDuration(orderTask, currentOrderExecutionTime + currentOrderReconfigurationTime);
                    _projectManager.SetComplete(orderTask, 1.0f);
                    _projectManager.Group(productionLineTask, orderTask);
                    _projectManager.Relate(lastOrderTask, orderTask);
                    lastOrderTask = orderTask;
                }
            }
            switch (_productionPlanTargetFunction.Type)
            {
                case ObjectiveFunction.Types.Cost:
                    _viewModel.TargetFunctionString = string.Format(LocalizationManager.Instance.GetValue("Window.Main.GanttChart.CostTargetFunction"), Math.Round(productionPlanExecutionPrice, 2));
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
                throw new System.Exception("ProjectManager of GanttChart is null");
            }
            _ganttChart.Init(_projectManager);
        }
    }
}
