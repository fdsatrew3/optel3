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

        private enum ColumnIndexes { OrderNumber, Width, QuantityInRunningMeter, FinishedGoods, Waste, RollsCount, PredefinedTime, FilmRecipe, PlanningEndDate, PriceOverdue, ParentCustomer }

        public OrdersExcelParser(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        protected override Order ParseRow(IExcelDataReader excelDataReader)
        {
            return new Order
            {
                OrderNumber = excelDataReader.GetFormattedValue(ColumnIndexes.OrderNumber),
                Width = Convert.ToDouble(excelDataReader.GetFormattedValue(ColumnIndexes.Width)),
                QuantityInRunningMeter = excelDataReader.GetDouble(ColumnIndexes.QuantityInRunningMeter),
                FinishedGoods = excelDataReader.GetDouble(ColumnIndexes.FinishedGoods),
                Waste = excelDataReader.GetDouble(ColumnIndexes.Waste),
                RollsCount = Convert.ToInt32(excelDataReader.GetValue(ColumnIndexes.RollsCount)),
                PredefinedTime = excelDataReader.GetNullabeDouble(ColumnIndexes.PredefinedTime),
                FilmRecipe = UnitOfWork.FilmRecipeRepository.GetSingle(x => x.Name == excelDataReader.GetFormattedValue(ColumnIndexes.FilmRecipe)),
                PlanningEndDate = System.DateTime.Parse(excelDataReader.GetValue(ColumnIndexes.PlanningEndDate).ToString()),
                PriceOverdue = excelDataReader.GetDouble(ColumnIndexes.PriceOverdue),
                ParentCustomer = UnitOfWork.CustomerRepository.GetSingle(x => x.Name == excelDataReader.GetFormattedValue(ColumnIndexes.ParentCustomer))
            };
        }
    }
}
