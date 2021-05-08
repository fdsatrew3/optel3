using OPTEL.UI.Desktop.Services.OpenFileDialogs;
using OPTEL.UI.Desktop.ViewModels;
using System.Windows;

namespace OPTEL.UI.Desktop.Views
{
    /// <summary>
    /// Логика взаимодействия для ImportExcelDataWindow.xaml
    /// </summary>
    public partial class ImportExcelDataWindow : Window
    {
        public ImportExcelDataWindow()
        {
            var ExcelOpenFileDialogService = new ExcelOpenFileDialogService(this);
            DataContext = new ImportExcelViewModel(ExcelOpenFileDialogService);
            InitializeComponent();
        }
    }
}
