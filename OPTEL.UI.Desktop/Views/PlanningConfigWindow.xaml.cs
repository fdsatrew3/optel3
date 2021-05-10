using OPTEL.Data;
using OPTEL.Optimization.Algorithms.Best;
using OPTEL.Optimization.Algorithms.TargetFunctionCalculators.Time;
using OPTEL.UI.Desktop.Models;
using OPTEL.UI.Desktop.Services.ErrorsListWindows;
using OPTEL.UI.Desktop.Services.GanttChartManagers;
using OPTEL.UI.Desktop.Services.ModelsConverter;
using OPTEL.UI.Desktop.ViewModels;
using System;
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
            var errorsListWindowService = new DefaultErrorListWindowService(this);
            var planningConfigOrderConverterService = new PlanningConfigOrderToOrderConverterService();
            var planningConfigProductionLineConverterService = new PlanningConfigProductionLineToProductionLineConverterService();
            var orderExecutionCalculatorServer = new OrderExcecutionTimeCalculator();
            var ordersReconfigurationTimeCalculator = new OrdersReconfigurationTimeCalculator();
            var ganttChartManagerService = new MainWindowGanttChartManagerService(mainWindow.GanttChart, orderExecutionCalculatorServer, ordersReconfigurationTimeCalculator);
            DataContext = new PlanningConfigViewModel(errorsListWindowService, planningConfigOrderConverterService, planningConfigProductionLineConverterService, ganttChartManagerService, this.tabControl.Items.Count - 1);
        }
    }
}
