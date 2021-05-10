using Braincase.GanttChart;
using OPTEL.Data;
using OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Time.Base;
using OPTEL.UI.Desktop.Services.GanttChartManagers.Base;
using System;

namespace OPTEL.UI.Desktop.Services.GanttChartManagers
{
    public class MainWindowGanttChartManagerService : IGanttChartManagerService
    {
        Chart _ganttChart;
        ProductionPlan _plan;
        ProjectManager _projectManager;
        DateTime _planningStartDate, _planningEndDate;
        IOrderExcecutionTimeCalculator _orderExecutionTimeCalculator;

        public MainWindowGanttChartManagerService(Chart ganttChart, IOrderExcecutionTimeCalculator orderExecutionTimeCalculator)
        {
            _ganttChart = ganttChart ?? throw new Exception("Chart is null");
            _planningStartDate = DateTime.MinValue;
            _planningEndDate = DateTime.MaxValue;
            _orderExecutionTimeCalculator = orderExecutionTimeCalculator ?? throw new Exception("IOrderExcecutionTimeCalculator is null");
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
            TimeSpan currentOrderExecutionTime;
            Task lastOrderTask;
            foreach (ProductionLineQueue queue in plan.ProductionLineQueues)
            {
                Task productionLineTask = new Task();
                productionLineTask.Name = queue.ProductionLine.Name;
                _projectManager.SetStart(productionLineTask, TimeSpan.FromHours(0));
                _projectManager.SetDuration(productionLineTask, productionPlanDuration);
                lastOrderTask = productionLineTask;
                currentTimeSpanOffset = TimeSpan.FromSeconds(0);
                foreach (Order order in queue.Orders)
                {
                    Task orderTask = new Task();
                    orderTask.Name = order.OrderNumber;
                    currentOrderExecutionTime = TimeSpan.FromMinutes(_orderExecutionTimeCalculator.Calculate(order));
                    currentTimeSpanOffset += currentOrderExecutionTime;
                    _projectManager.SetStart(orderTask, currentTimeSpanOffset);
                    _projectManager.SetDuration(orderTask, currentOrderExecutionTime);
                    _projectManager.SetComplete(orderTask, 1.0f);
                    _projectManager.Group(productionLineTask, orderTask);
                    _projectManager.Relate(lastOrderTask, orderTask);
                    lastOrderTask = orderTask;
                }
            }
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
