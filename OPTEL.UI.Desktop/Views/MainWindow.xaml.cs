﻿using Braincase.GanttChart;
using OPTEL.UI.Desktop.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

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
            ProjectManager projectManager = new ProjectManager();
            GanttChart.Init(projectManager);
            Application.Current.MainWindow = this;
        }
    }
}
