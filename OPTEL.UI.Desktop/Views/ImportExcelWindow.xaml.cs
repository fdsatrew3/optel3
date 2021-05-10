using OPTEL.UI.Desktop.Services.ErrorsListWindows;
using OPTEL.UI.Desktop.Services.ExcelDataReaders;
using OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers;
using OPTEL.UI.Desktop.Services.OpenFileDialogs;
using OPTEL.UI.Desktop.Services.WindowClosers;
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
            InitializeComponent();
            var filmTypesExcelParser = new FilmTypesExcelParser();
            var filmRecipesExcelParser = new FilmRecipesExcelParser(Database.instance);
            var customersExcelParser = new CustomersExcelParser();
            var ordersExcelParser = new OrdersExcelParser(Database.instance);
            var productionLinesExcelParser = new ProductionLinesExcelParser();
            var filmTypesChangesExcelParser = new FilmTypesChangesExcelParser(Database.instance);
            var nozzleChangesExcelParser = new NozzleChangesExcelParser(Database.instance);
            var calibrationChangesExcelParser = new CalibrationChangesExcelParser(Database.instance);
            var coolingLipChangesExcelParser = new CoolingLipChangesExcelParser(Database.instance);
            var excelDataReaderService = new ExcelEntityReader(Database.instance,
                 filmTypesExcelParser,
                 filmRecipesExcelParser,
                 customersExcelParser,
                 ordersExcelParser,
                 productionLinesExcelParser,
                 filmTypesChangesExcelParser,
                 nozzleChangesExcelParser,
                 calibrationChangesExcelParser,
                 coolingLipChangesExcelParser);

            var excelOpenFileDialogService = new ExcelOpenFileDialogService(this);
            var windowCloseService = new DatabaseEntityWindowCloseService(this);
            var errorsListWindowService = new DefaultErrorListWindowService(this);
            DataContext = new ImportExcelViewModel(excelOpenFileDialogService, excelDataReaderService, windowCloseService, errorsListWindowService);
        }
    }
}
