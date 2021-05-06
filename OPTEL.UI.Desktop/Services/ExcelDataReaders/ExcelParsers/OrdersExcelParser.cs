using ExcelDataReader;
using OPTEL.Data;
using OPTEL.Entity.Core;

using OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers.Base;
using Convert = System.Convert;

namespace OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers
{
    public class OrdersExcelParser : DatabaseExcelParcerBase<Order>
    {
        protected override int WorkSheetIndex => 4;

        private enum ColumnIndexes { OrderNumber, Width, QuantityInRunningMeter, FilmRecipe, PlanningEndDate, PriceOverdue, ParentCustomer }

        public OrdersExcelParser(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        protected override Order ParseRow(IExcelDataReader excelDataReader)
        {
            return new Order
            {
                OrderNumber = excelDataReader.GetValue((int)ColumnIndexes.OrderNumber).ToString(),
                Width = Convert.ToDouble(excelDataReader.GetValue((int)ColumnIndexes.Width).ToString()),
                QuantityInRunningMeter = Convert.ToDouble(excelDataReader.GetValue((int)ColumnIndexes.QuantityInRunningMeter)),
                FilmRecipe = UnitOfWork.FilmRecipeRepository.GetSingle(x => x.Name == excelDataReader.GetValue((int)ColumnIndexes.FilmRecipe).ToString()),
                PlanningEndDate = System.DateTime.Parse(excelDataReader.GetValue((int)ColumnIndexes.PlanningEndDate).ToString()),
                PriceOverdue = Convert.ToDouble(excelDataReader.GetValue((int)ColumnIndexes.PriceOverdue)),
                ParentCustomer = UnitOfWork.CustomerRepository.GetSingle(x => x.Name == excelDataReader.GetValue((int)ColumnIndexes.ParentCustomer).ToString())
            };
        }
    }
}
