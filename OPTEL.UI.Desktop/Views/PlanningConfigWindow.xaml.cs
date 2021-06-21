using OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Cost;
using OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Time;
using OPTEL.UI.Desktop.Services.ErrorsListWindows;
using OPTEL.UI.Desktop.Services.GanttChartManagers;
using OPTEL.UI.Desktop.Services.ModelsConverter;
using OPTEL.UI.Desktop.Services.WindowClosers;
using OPTEL.UI.Desktop.ViewModels;
using System.Windows;

namespace OPTEL.UI.Desktop.Views
{
    /// <summary>
    /// Логика взаимодействия для PlanningConfigWindow.xaml.xaml
    /// </summary>
    public partial class PlanningConfigWindow : Window
    {
        public PlanningConfigWindow()
        {
            InitializeComponent();
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            var errorsListWindowService = new GenericErrorListWindowService(this);
            var planningConfigOrderConverterService = new PlanningConfigOrderToOrderConverterService();
            var planningConfigProductionLineConverterService = new PlanningConfigProductionLineToProductionLineConverterService();
            var orderExecutionCalculatorServer = new OrderExcecutionTimeCalculator();
            var ordersReconfigurationTimeCalculator = new OrdersReconfigurationTimeCalculator();
            var windowCloseService = new GenericWindowCloseService(this);
            var orderExecutionTimeCalculator = new OrderExcecutionTimeCalculator();
            var orderReconfigurationTimeCalculator = new OrdersReconfigurationTimeCalculator();
            var executionTimeCalculator = new ExecutionTimeCalculator(orderExecutionTimeCalculator);
            var reconfigurationTimeCalculator = new ReconfigurationTimeCalculator(orderReconfigurationTimeCalculator);
            var productionLineQueueTimeCalculator = new ProductionLineQueueTimeCalculator(executionTimeCalculator, reconfigurationTimeCalculator);
            var orderCostCalculator = new OrderCostCalculator();
            var ordersReconfigurationCostCalculator = new OrdersReconfigurationCostCalculator();
            var reconfigurationCostCalculator = new ReconfigurationCostCalculator(ordersReconfigurationCostCalculator);
            var ganttChartManagerService = new MainWindowGanttChartManagerService(mainWindow.DataContext as MainWindowViewModel, mainWindow.GanttChart, orderExecutionCalculatorServer, ordersReconfigurationTimeCalculator, productionLineQueueTimeCalculator);

            DataContext = new PlanningConfigViewModel(errorsListWindowService,
                planningConfigOrderConverterService,
                planningConfigProductionLineConverterService,
                ganttChartManagerService,
                windowCloseService,
                productionLineQueueTimeCalculator,
                tabControl.Items.Count - 1,
                OrdersList,
                ProductionLinesList,
                orderCostCalculator,
                reconfigurationCostCalculator);
        }
    }
}
