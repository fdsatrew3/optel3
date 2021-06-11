using EasyLocalization.Localization;
using EasyLocalization.Readers;
using OPTEL.Data.Users;
using OPTEL.Entity.Helpers.Ensurers;
using OPTEL.UI.Desktop.Helpers;
using OPTEL.UI.Desktop.Views;
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
        private void OnApplicationStartup(object sender, StartupEventArgs e)
        {
            LoadLocalizationFiles();
            Database.instance = new Entity.Persistance.UnitOfWork(new ProductionDataBaseEnsurer());
            AddDefaultAdminToEmptyDatabase();
            ShowMainWindow();
            ShowLoginWindow();
        }
        private void ShowMainWindow()
        {
            MainWindow window = new MainWindow();
            window.Show();
        }

        private void ShowLoginWindow()
        {
            LoginWindow window = new LoginWindow();
            window.ShowModalDialog();
        }

        private void AddDefaultAdminToEmptyDatabase()
        {
            if (Database.instance.AdministratorRepository.GetCount() == 0)
            {
                Administrator admin = new Administrator();
                admin.Login = "admin";
                admin.Password = LoginWindow.Encrypt("admin");
                Database.instance.AdministratorRepository.Add(admin);
                Database.instance.Save();
            }
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
