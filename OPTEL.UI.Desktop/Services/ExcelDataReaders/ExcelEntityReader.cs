using System;
using System.Collections.Generic;
using OPTEL.Entity.Core;
using OPTEL.Data;

using OPTEL.UI.Desktop.Services.Data;
using OPTEL.UI.Desktop.Services.ExcelDataReaders.Base;
using OPTEL.UI.Desktop.Services.ExcelDataReaders.ExcelParsers.Base;
using System.IO;
using ExcelDataReader;

namespace OPTEL.UI.Desktop.Services.ExcelDataReaders
{
    public class ExcelEntityReader : IExcelEntityReader
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

        public ExcelEntityReader(IUnitOfWork unitOfWork,
            IExcelParcer<FilmType> filmTypesExcelParser,
            IExcelParcer<FilmRecipe> filmRecipesExcelParser,
            IExcelParcer<Customer> customersExcelParser,
            IExcelParcer<Order> ordersExcelParser,
            IExcelParcer<ProductionLine> productionLinesExcelParser,
            IExcelParcer<FilmTypesChange> filmTypesChangesExcelParser,
            IExcelParcer<NozzleChange> nozzleChangesExcelParser,
            IExcelParcer<CalibrationChange> calibrationChangesExcelParser,
            IExcelParcer<CoolingLipChange> coolingLipChangesExcelParser)
        {
            _filmTypesExcelParser = filmTypesExcelParser ?? throw new ArgumentNullException(nameof(filmTypesExcelParser));
            _filmRecipesExcelParser = filmRecipesExcelParser ?? throw new ArgumentNullException(nameof(filmRecipesExcelParser));
            _customersExcelParser = customersExcelParser ?? throw new ArgumentNullException(nameof(customersExcelParser));
            _ordersExcelParser = ordersExcelParser ?? throw new ArgumentNullException(nameof(ordersExcelParser));
            _productionLinesExcelParser = productionLinesExcelParser ?? throw new ArgumentNullException(nameof(productionLinesExcelParser));
            _filmTypesChangesExcelParser = filmTypesChangesExcelParser ?? throw new ArgumentNullException(nameof(filmTypesChangesExcelParser));
            _nozzleChangesExcelParser = nozzleChangesExcelParser ?? throw new ArgumentNullException(nameof(nozzleChangesExcelParser));
            _calibrationChangesExcelParser = calibrationChangesExcelParser ?? throw new ArgumentNullException(nameof(calibrationChangesExcelParser));
            _coolingLipChangesExcelParser = coolingLipChangesExcelParser ?? throw new ArgumentNullException(nameof(coolingLipChangesExcelParser));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public IEnumerable<ActionResult> ReadFile(string filePath)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            using var reader = ExcelReaderFactory.CreateReader(stream);

            foreach (var item in ParseWorkBookAndSave(reader, _filmTypesExcelParser, _unitOfWork.FilmTypeRepository, x => $"Film type {x.Article}"))
            {
                yield return item;
            }

            foreach (var item in ParseWorkBookAndSave(reader, _filmRecipesExcelParser, _unitOfWork.FilmRecipeRepository, x => $"film recipe {x.Name}"))
            {
                yield return item;
            }

            foreach (var item in ParseWorkBookAndSave(reader, _customersExcelParser, _unitOfWork.CustomerRepository, x => $"customer {x.Name}"))
            {
                yield return item;
            }

            foreach (var item in ParseWorkBookAndSave(reader, _ordersExcelParser, _unitOfWork.OrderRepository, x => $"order {x.OrderNumber}"))
            {
                yield return item;
            }

            foreach (var item in ParseWorkBookAndSave(reader, _productionLinesExcelParser, _unitOfWork.ProductionLineRepository, x => $"production line {x.Name}"))
            {
                yield return item;
            }

            foreach (var item in ParseWorkBookAndSave(reader, _filmTypesChangesExcelParser, _unitOfWork.FilmRecipeChangeRepository, x => $"type change"))
            {
                yield return item;
            }

            foreach (var item in ParseWorkBookAndSave(reader, _nozzleChangesExcelParser, _unitOfWork.NozzleChangeRepository, x => $"nozzle change"))
            {
                yield return item;
            }

            foreach (var item in ParseWorkBookAndSave(reader, _calibrationChangesExcelParser, _unitOfWork.CalibrationChangeRepository, x => $"calibration change"))
            {
                yield return item;
            }

            foreach (var item in ParseWorkBookAndSave(reader, _coolingLipChangesExcelParser, _unitOfWork.CoolingLipChangeRepository, x => $"cooling lip change"))
            {
                yield return item;
            }
        }

        private IEnumerable<ActionResult> ParseWorkBookAndSave<T>(IExcelDataReader excelDataReader, IExcelParcer<T> excelParcer, IRepository<T> repository, Func<T, string> exceptionName)
            where T : class, OPTEL.Data.Core.IDataObject
        {
            int i = excelParcer.StartRowIndex;

            foreach (var actionResult in excelParcer.ParseFile(excelDataReader))
            {
                var result = new ActionResult();

                if (actionResult.HasException)
                {
                    result.Exception = actionResult.Exception;
                }
                else
                {
                    repository.Add(actionResult.Result);

                    try
                    {
                        _unitOfWork.Save();
                    }
                    catch (Exception ex)
                    {
                        result.Exception = new Exception($"(row:{i}) There is some error to save entity {exceptionName?.Invoke(actionResult.Result)}", ex);
                    }
                }

                yield return result;
                i++;
            }
        }
    }
}
