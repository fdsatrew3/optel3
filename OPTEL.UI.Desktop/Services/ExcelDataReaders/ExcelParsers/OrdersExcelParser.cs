using ExcelDataReader;
using OPTEL.Data;
using OPTEL.Entity.Core;

using OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers.Base;
using OPTEL.UI.Desktop.Services.ExcelDataReaders.Utils;
using Convert = System.Convert;

namespace OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers
{
    public class OrdersExcelParser : DatabaseExcelParcerBase<Order>
    {
        protected override int WorkSheetIndex => 4;

        private enum ColumnIndexes { OrderNumber, Width, QuantityInRunningMeter, FilmRecipe, PlanningEndDate, PriceOverdue, ParentCustomer, PredefinedTime }

        public OrdersExcelParser(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        protected override Order ParseRow(IExcelDataReader excelDataReader)
        {
            return new Order
            {
                OrderNumber = excelDataReader.GetFormattedValue(ColumnIndexes.OrderNumber),
                Width = Convert.ToDouble(excelDataReader.GetFormattedValue(ColumnIndexes.Width)),
                QuantityInRunningMeter = Convert.ToDouble(excelDataReader.GetValue(ColumnIndexes.QuantityInRunningMeter)),
                FilmRecipe = UnitOfWork.FilmRecipeRepository.GetSingle(x => x.Name == excelDataReader.GetFormattedValue(ColumnIndexes.FilmRecipe)),
                PlanningEndDate = System.DateTime.Parse(excelDataReader.GetValue(ColumnIndexes.PlanningEndDate).ToString()),
                PriceOverdue = Convert.ToDouble(excelDataReader.GetValue(ColumnIndexes.PriceOverdue)),
                ParentCustomer = UnitOfWork.CustomerRepository.GetSingle(x => x.Name == excelDataReader.GetFormattedValue(ColumnIndexes.ParentCustomer)),
                PredefinedTime = Convert.ToDouble(excelDataReader.GetValue(ColumnIndexes.PredefinedTime))
            };
        }
    }
}
