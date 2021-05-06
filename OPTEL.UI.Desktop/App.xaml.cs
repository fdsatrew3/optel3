using EasyLocalization.Localization;
using EasyLocalization.Readers;
using OPTEL.Entity.Helpers.Ensurers;
using System;
using System.Globalization;
using System.IO;
using System.Windows;

namespace OPTEL.UI.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            LoadLocalizationFiles();
            UnitOfWork.instance = new Entity.Persistance.UnitOfWork(new ProductionDataBaseEnsurer());
            Data.FilmRecipe filmRecipe = new Data.FilmRecipe
            {
                ID = 0,
                FilmType = UnitOfWork.instance.FilmTypeRepository.GetFirst(),
                Calibration = 2m,
                CoolingLip = 3m,
                MaterialCost = 4m,
                Name = "123",
                Nozzle = 5m,
                ProductionSpeed = 6m,
                Thickness = 7m
            };
            UnitOfWork.instance.FilmRecipeRepository.Add(filmRecipe);
            UnitOfWork.instance.Save();

            //UnitOfWork.instance.CalibrationChangeRepository.Add(new Data.CalibrationChange { CalibrationToChange = 100, ID = 0, ParentProductionLine = productionLine, ReconfigurationTime = 20 });
        }

        private void LoadLocalizationFiles()
        {
            string[] localizationFiles;
            try
            {
                localizationFiles = Directory.GetFiles("./Resources/Localization/", "*.*", SearchOption.TopDirectoryOnly);
            }
            catch
            {
                return;
            }
            for (var i = 0; i < localizationFiles.Length; i++)
            {
                try
                {
                    string fileNameWithoutExtension = Path.GetFileName(localizationFiles[i]).Replace(".json", String.Empty);
                    CultureInfo fileCulture = CultureInfo.GetCultureInfo(fileNameWithoutExtension);
                    bool isDefaultCulture = false;
                    if (fileNameWithoutExtension.Equals("en-US"))
                    {
                        isDefaultCulture = true;
                    }
                    LocalizationManager.Instance.AddCulture(
                        fileCulture,
                        new JsonFileReader(localizationFiles[i]),
                        isDefaultCulture);
                }
                catch
                {
                    continue;
                }
            }
        }
    }
}
