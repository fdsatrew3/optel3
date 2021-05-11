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
            Database.instance = new Entity.Persistance.UnitOfWork(new ProductionDataBaseEnsurer());
            /*Data.ProductionLine line = new Data.ProductionLine {
                ID = 0,
                Code = "09",
                Name = "MEX 09",
                HourCost = 40,
                WidthChangeTime = 20,
                ThicknessChangeTime = 20,
                LengthMax = 999,
                LengthMin = 10,
                MaxProductionSpeed = 400
                
            };
            Database.instance.ProductionLineRepository.Add(line);
            Data.CalibrationChange calibration = Database.instance.CalibrationChangeRepository.GetFirst();
            calibration.CalibrationToChange = 200;
            calibration.ParentProductionLine = line;
            Database.instance.CalibrationChangeRepository.Update(calibration);
            Database.instance.Save(); */
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
