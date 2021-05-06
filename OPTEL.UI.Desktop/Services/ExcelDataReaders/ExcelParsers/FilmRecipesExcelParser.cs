using ExcelDataReader;
using OPTEL.Data;
using OPTEL.Entity.Core;

using OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers.Base;
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
                Name = excelDataReader.GetValue((int)ColumnIndexes.Name).ToString(),
                FilmType = UnitOfWork.FilmTypeRepository.GetSingle(x => x.Article == excelDataReader.GetValue((int)ColumnIndexes.FilmType).ToString()),
                Thickness = Convert.ToDouble(excelDataReader.GetValue((int)ColumnIndexes.Thickness)),
                ProductionSpeed = Convert.ToDouble(excelDataReader.GetValue((int)ColumnIndexes.ProductionSpeed)),
                MaterialCost = Convert.ToDouble(excelDataReader.GetValue((int)ColumnIndexes.MaterialCost)),
                Nozzle = Convert.ToDouble(excelDataReader.GetValue((int)ColumnIndexes.Nozzle)),
                Calibration = Convert.ToDouble(excelDataReader.GetValue((int)ColumnIndexes.Calibration)),
                CoolingLip = Convert.ToDouble(excelDataReader.GetValue((int)ColumnIndexes.CoolingLip))
            };
        }
    }
}
