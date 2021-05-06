using OPTEL.Data;
using OPTEL.Entity.Core;
using OPTEL.Entity.Helpers.Ensurers;
using OPTEL.Entity.Persistance;
using OPTEL.UI.Desktop.Services.ExcelDataReaders;
using OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers;
using OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace OPTEL.UI.Desktop.Test.Services.ExcelDataReaders
{
    public class ExcelDataReaderIntegrationTest
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExcelParcer<FilmType> _filmTypesExcelParser;
        private readonly IExcelParcer<FilmRecipe> _filmRecipesExcelParser;
        private readonly IExcelParcer<Customer> _customersExcelParser;
        private readonly IExcelParcer<Order> _ordersExcelParser;
        private readonly IExcelParcer<ProductionLine> _productionLinesExcelParser;
        private readonly IExcelParcer<FilmTypesChange> _filmTypesChangesExcelParser;
        private readonly IExcelParcer<NozzleChange> _nozzleChangesExcelParser;
        private readonly IExcelParcer<CalibrationChange> _calibrationChangesExcelParser;
        private readonly IExcelParcer<CoolingLipChange> _coolingLipChangesExcelParser;
        private readonly ExcelEntityReader _excelDataReader;

        public ExcelDataReaderIntegrationTest()
        {
            _unitOfWork = new UnitOfWork(new TestingDataBaseEnsurer());
            _filmTypesExcelParser = new FilmTypesExcelParser();
            _filmRecipesExcelParser = new FilmRecipesExcelParser(_unitOfWork);
            _customersExcelParser = new CustomersExcelParser();
            _ordersExcelParser = new OrdersExcelParser(_unitOfWork);
            _productionLinesExcelParser = new ProductionLinesExcelParser();
            _filmTypesChangesExcelParser = new FilmTypesChangesExcelParser(_unitOfWork);
            _nozzleChangesExcelParser = new NozzleChangesExcelParser(_unitOfWork);
            _calibrationChangesExcelParser = new CalibrationChangesExcelParser(_unitOfWork);
            _coolingLipChangesExcelParser = new CoolingLipChangesExcelParser(_unitOfWork);
            _excelDataReader = new ExcelEntityReader(_unitOfWork,
                _filmTypesExcelParser,
                _filmRecipesExcelParser,
                _customersExcelParser,
                _ordersExcelParser,
                _productionLinesExcelParser,
                _filmTypesChangesExcelParser,
                _nozzleChangesExcelParser,
                _calibrationChangesExcelParser,
                _coolingLipChangesExcelParser);
        }

        [Theory]
        [InlineData(@"\Assets\TestExample.xlsx")]
        public void ReadFile_WithTemplateFile_ReadAndSaveAllEntitiesInDatabase(string file)
        {
            // Arrange
            var path = Environment.CurrentDirectory + file;

            // Act && Assert
            foreach(var item in _excelDataReader.ReadFile(path))
            {
                Assert.False(item.HasException);
            }
        }
    }
}
