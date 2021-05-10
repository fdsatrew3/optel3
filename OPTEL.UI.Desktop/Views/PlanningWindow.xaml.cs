using OPTEL.Data;
using OPTEL.Optimization.Algorithms.Best;
using System;
using System.Windows;

namespace OPTEL.UI.Desktop.Views
{
    /// <summary>
    /// Логика взаимодействия для PlanningWindow.xaml
    /// </summary>
    public partial class PlanningWindow : Window
    {
        public PlanningWindow()
        {
            InitializeComponent();
            //var windowCloseService = new DatabaseEntityWindowCloseService(this);
            //var errorsListWindowService = new DefaultErrorListWindowService(this);
            //DataContext = new CustomersViewModel(windowCloseService, errorsListWindowService);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var prodLines = Database.instance.ProductionLineRepository.GetAll();
            var orders = Database.instance.OrderRepository.GetAll();
            var filmChanges = Database.instance.FilmRecipeChangeRepository.GetAll();
            BestAlgorithm algh = new BestAlgorithm(prodLines, orders, filmChanges);
            ProductionPlan plan = algh.GetResolve();
            Console.WriteLine(1);
        }
    }
}
