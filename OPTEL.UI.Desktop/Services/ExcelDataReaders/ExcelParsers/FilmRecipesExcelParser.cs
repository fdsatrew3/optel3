using ExcelDataReader;
using OPTEL.Data;
using OPTEL.Entity.Core;

using OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers.Base;
using OPTEL.UI.Desktop.Services.ExcelDataReaders.Utils;
using Convert = System.Convert;

namespace OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers
{
    public class FilmRecipesExcelParser : DatabaseExcelParcerBase<FilmRecipe>
    {
        protected override int WorkSheetIndex => 2;

        private enum ColumnIndexes { Name, FilmType, Thickness, ProductionSpeed, MaterialCost, Nozzle, Calibration, CoolingLip }

        public FilmRecipesExcelParser(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        protected override FilmRecipe ParseRow(IExcelDataReader excelDataReader)
        {
            return new FilmRecipe
            {
                Name = excelDataReader.GetFormattedValue(ColumnIndexes.Name),
                FilmType = UnitOfWork.FilmTypeRepository.GetSingle(x => x.Article == excelDataReader.GetFormattedValue(ColumnIndexes.FilmType)),
                Thickness = excelDataReader.GetDouble(ColumnIndexes.Thickness),
                ProductionSpeed = excelDataReader.GetDouble(ColumnIndexes.ProductionSpeed),
                MaterialCost = excelDataReader.GetDouble(ColumnIndexes.MaterialCost),
                Nozzle = excelDataReader.GetDouble(ColumnIndexes.Nozzle),
                Calibration = excelDataReader.GetDouble(ColumnIndexes.Calibration),
                CoolingLip = excelDataReader.GetDouble(ColumnIndexes.CoolingLip)
            };
        }
    }
}
