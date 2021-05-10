using Braincase.GanttChart;
using OPTEL.Data;
using OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Time;
using OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Time.Base;
using OPTEL.UI.Desktop.Services.GanttChartManagers.Base;
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

        public MainWindowGanttChartManagerService(Chart ganttChart, IOrderExcecutionTimeCalculator orderExecutionTimeCalculator, IOrdersReconfigurationTimeCalculator ordersReconfigurationTimeCalculator)
        {
            _ganttChart = ganttChart ?? throw new Exception("Chart is null");
            _planningStartDate = DateTime.MinValue;
            _planningEndDate = DateTime.MaxValue;
            _orderExecutionTimeCalculator = orderExecutionTimeCalculator ?? throw new Exception("IOrderExcecutionTimeCalculator is null");
            _ordersReconfigurationTimeCalculator = ordersReconfigurationTimeCalculator;
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
            Debug.WriteLine("=========================================== 2 NOCHI BLYAT");
            foreach (ProductionLineQueue queue in plan.ProductionLineQueues)
            {
                Task productionLineTask = new Task();
                productionLineTask.Name = queue.ProductionLine.Name;
                _projectManager.SetStart(productionLineTask, TimeSpan.FromHours(0));
                _projectManager.SetDuration(productionLineTask, productionPlanDuration);
                lastOrderTask = productionLineTask;
                currentTimeSpanOffset = TimeSpan.FromSeconds(0);
                lastOrder = null;
                foreach (Order order in queue.Orders)
                {
                    if(lastOrder == null)
                    {
                        lastOrder = order;
                    }
                    Task orderTask = new Task();
                    orderTask.Name = order.OrderNumber;
                    currentOrderExecutionTime = TimeSpan.FromMinutes(_orderExecutionTimeCalculator.Calculate(order));
                    currentOrderReconfigurationTime = TimeSpan.FromMinutes(_ordersReconfigurationTimeCalculator.Calculate(queue.ProductionLine, lastOrder, order));
                    Debug.WriteLine(currentOrderExecutionTime.ToString());
                    currentTimeSpanOffset += currentOrderExecutionTime + currentOrderReconfigurationTime;
                    _projectManager.SetStart(orderTask, currentTimeSpanOffset);
                    //_projectManager.SetDuration(orderTask, currentOrderExecutionTime);
                    _projectManager.SetDuration(orderTask, TimeSpan.FromHours(8));
                    _projectManager.SetComplete(orderTask, 1.0f);
                    _projectManager.Group(productionLineTask, orderTask);
                    _projectManager.Relate(lastOrderTask, orderTask);
                    lastOrderTask = orderTask;
                }
            }
            /*
            ProjectManager _mManager = null;
            _mManager = new ProjectManager();
            var work = new MyTask(_mManager) { Name = "Prepare for Work" };
            var wake = new MyTask(_mManager) { Name = "Wake Up" };
            var teeth = new MyTask(_mManager) { Name = "Brush Teeth" };
            var shower = new MyTask(_mManager) { Name = "Shower" };
            var clothes = new MyTask(_mManager) { Name = "Change into New Clothes" };
            var hair = new MyTask(_mManager) { Name = "Blow My Hair" };
            var pack = new MyTask(_mManager) { Name = "Pack the Suitcase" };

            _mManager.Add(work);
            _mManager.Add(wake);
            _mManager.Add(teeth);
            _mManager.Add(shower);
            _mManager.Add(clothes);
            _mManager.Add(hair);
            _mManager.Add(pack);

            // Create another 1000 tasks for stress testing
            Random rand = new Random();
            for (int i = 0; i < 1000; i++)
            {
                var task = new MyTask(_mManager) { Name = string.Format("New Task {0}", i.ToString()) };
                _mManager.Add(task);
                _mManager.SetStart(task, TimeSpan.FromDays(rand.Next(300)));
                _mManager.SetDuration(task, TimeSpan.FromDays(rand.Next(50)));
            }

            // Set task durations, e.g. using ProjectManager methods 
            _mManager.SetDuration(wake, TimeSpan.FromDays(3));
            _mManager.SetDuration(teeth, TimeSpan.FromDays(5));
            _mManager.SetDuration(shower, TimeSpan.FromDays(7));
            _mManager.SetDuration(clothes, TimeSpan.FromDays(4));
            _mManager.SetDuration(hair, TimeSpan.FromDays(3));
            _mManager.SetDuration(pack, TimeSpan.FromDays(5));

            // demostrate splitting a task
            // Set task complete status, e.g. using newly created properties
            wake.Complete = 0.9f;
            teeth.Complete = 0.5f;
            shower.Complete = 0.4f;

            // Give the Tasks some organisation, setting group and precedents
            _mManager.Group(work, wake);
            _mManager.Group(work, teeth);
            _mManager.Group(work, shower);
            _mManager.Group(work, clothes);
            _mManager.Group(work, hair);
            _mManager.Group(work, pack);
            _mManager.Relate(wake, teeth);
            _mManager.Relate(wake, shower);
            _mManager.Relate(shower, clothes);
            _mManager.Relate(shower, hair);
            _mManager.Relate(hair, pack);
            _mManager.Relate(clothes, pack);

            // Initialize the Chart with our ProjectManager and CreateTaskDelegate
            _projectManager = _mManager; */
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
