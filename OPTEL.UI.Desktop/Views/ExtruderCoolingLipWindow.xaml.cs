﻿using OPTEL.UI.Desktop.ViewModels;
using OPTEL.UI.Desktop.Views.Core;

namespace OPTEL.UI.Desktop.Views
{
    /// <summary>
    /// Логика взаимодействия для ExtruderCoolingLipWindow.xaml
    /// </summary>
    public partial class ExtruderCoolingLipWindow : DatabaseEntityView
    {
        public ExtruderCoolingLipWindow()
        {
            InitializeComponent();
            this.DataContext = new ExtruderCoolingLipsViewModel();
        }

        public override void SetSelectedItem(object item)
        {
            ElementsList.SelectedItem = item;
        }
    }
}
