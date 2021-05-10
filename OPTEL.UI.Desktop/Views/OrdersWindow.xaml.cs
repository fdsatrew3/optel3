﻿using OPTEL.UI.Desktop.Services.WindowClosers;
using OPTEL.UI.Desktop.ViewModels;
using OPTEL.UI.Desktop.Views.Core;

namespace OPTEL.UI.Desktop.Views
{
    /// <summary>
    /// Логика взаимодействия для OrdersWindow.xaml
    /// </summary>
    public partial class OrdersWindow : DatabaseEntityView
    {
        public OrdersWindow()
        {
            InitializeComponent();
            var windowCloseService = new DatabaseEntityWindowCloseService(this);
            this.DataContext = new OrdersViewModel(windowCloseService);
        }

        public override void SetSelectedItem(object item)
        {
            ElementsList.SelectedItem = item;
        }
    }
}
