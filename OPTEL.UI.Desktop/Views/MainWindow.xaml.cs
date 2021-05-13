using Braincase.GanttChart;
using OPTEL.UI.Desktop.ViewModels;
using System.Drawing;
using System.Windows;
using SystemColors = System.Drawing.SystemColors;

namespace OPTEL.UI.Desktop.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            //GanttChart.TimeResolution = TimeResolution.Hour;
            ProjectManager projectManager = new ProjectManager();
            GanttChart.Init(projectManager);
            Application.Current.MainWindow = this;
        }
    }
}
