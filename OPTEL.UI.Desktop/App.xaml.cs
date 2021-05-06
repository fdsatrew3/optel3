using EasyLocalization.Localization;
using EasyLocalization.Readers;
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
