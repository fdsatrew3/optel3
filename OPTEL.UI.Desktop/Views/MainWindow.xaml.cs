using Braincase.GanttChart;
using OPTEL.UI.Desktop.Models;
using OPTEL.UI.Desktop.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows;

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
            GanttChart.TimeResolution = TimeResolution.Hour;
            ProjectManager projectManager = new ProjectManager();
            GanttChart.Init(projectManager);
            Application.Current.MainWindow = this;
        }
    }
}
