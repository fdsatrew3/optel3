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
                Thickness = Convert.ToDouble(excelDataReader.GetValue(ColumnIndexes.Thickness)),
                ProductionSpeed = Convert.ToDouble(excelDataReader.GetValue(ColumnIndexes.ProductionSpeed)),
                MaterialCost = Convert.ToDouble(excelDataReader.GetValue(ColumnIndexes.MaterialCost)),
                Nozzle = Convert.ToDouble(excelDataReader.GetValue(ColumnIndexes.Nozzle)),
                Calibration = Convert.ToDouble(excelDataReader.GetValue(ColumnIndexes.Calibration)),
                CoolingLip = Convert.ToDouble(excelDataReader.GetValue(ColumnIndexes.CoolingLip))
            };
        }
    }
}
