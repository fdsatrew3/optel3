using EasyLocalization.Localization;
using OPTEL.Data;
using OPTEL.UI.Desktop.Helpers;
using OPTEL.UI.Desktop.Services.WindowClosers.Base;
using OPTEL.UI.Desktop.ViewModels.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace OPTEL.UI.Desktop.ViewModels
{
    public class FilmRecipesViewModel : DatabaseEntityViewModel
    {
        #region Properties
        public FilmRecipe SelectedFilmRecipe
        {
            get => _SelectedFilmRecipe;
            set
            {
                _SelectedFilmRecipe = value;
                OnPropertyChanged("SelectedFilmRecipe");
            }
        }
        public ObservableCollection<FilmRecipe> FilmRecipes { get; set; }
        public IEnumerable<FilmType> FilmTypes { get; set; }
        #endregion
        #region Fields
        private FilmRecipe _SelectedFilmRecipe;

        private RelayCommand _selectFirstDataEntryIfExistsCommand;
        private RelayCommand _addEntityCommand;
        private RelayCommand _removeEntityCommand;
        private RelayCommand _cloneEntityCommand;
        #endregion
        public FilmRecipesViewModel(IWindowCloseService windowCloseService) : base(windowCloseService)
        {
            FilmRecipes = new ObservableCollection<FilmRecipe>(Database.instance.FilmRecipeRepository.GetAll());
            FilmTypes = Database.instance.FilmTypeRepository.GetAll();
        }

        #region Commands
        public RelayCommand SelectFirstDataEntryIfExistsCommand
        {
            get
            {
                return _selectFirstDataEntryIfExistsCommand ??= new RelayCommand(obj =>
                {
                    FilmRecipe coolingLip;
                    try
                    {
                        coolingLip = FilmRecipes.First();
                    }
                    catch
                    {
                        return;
                    }
                    SelectedFilmRecipe = coolingLip;
                });
            }
        }
        public RelayCommand AddEntityCommand
        {
            get
            {
                return _addEntityCommand ??= new RelayCommand(obj =>
                {
                    FilmRecipe change = new FilmRecipe();
                    FilmRecipes.Add(change);
                    Database.instance.FilmRecipeRepository.Add(change);
                    SelectedFilmRecipe = change;
                });
            }
        }
        public RelayCommand RemoveEntityCommand
        {
            get
            {
                return _removeEntityCommand ??= new RelayCommand(obj =>
                {
                    Database.instance.FilmRecipeRepository.Delete(SelectedFilmRecipe);
                    FilmRecipes.Remove(SelectedFilmRecipe);
                    SelectFirstDataEntryIfExistsCommand.Execute(null);
                }, (obj) => SelectedFilmRecipe != null);
            }
        }
        public RelayCommand CloneEntityCommand
        {
            get
            {
                return _cloneEntityCommand ??= new RelayCommand(obj =>
                {
                    FilmRecipe change = new FilmRecipe();
                    change.Calibration = SelectedFilmRecipe.Calibration;
                    change.CoolingLip = SelectedFilmRecipe.CoolingLip;
                    change.FilmType = SelectedFilmRecipe.FilmType;
                    change.FilmTypeID = SelectedFilmRecipe.FilmTypeID;
                    change.MaterialCost = SelectedFilmRecipe.MaterialCost;
                    change.Name = SelectedFilmRecipe.Name;
                    change.Nozzle = SelectedFilmRecipe.Nozzle;
                    change.ProductionSpeed = SelectedFilmRecipe.ProductionSpeed;
                    change.Thickness = SelectedFilmRecipe.Thickness;
                    FilmRecipes.Add(change);
                    Database.instance.FilmRecipeRepository.Add(change);
                    SelectedFilmRecipe = change;
                }, (obj) => SelectedFilmRecipe != null);
            }
        }
        #endregion
        public override string GetCustomErrorString()
        {
            string result = string.Empty;
            StringBuilder sb = new StringBuilder(result);
            for (int i = 0; i < FilmRecipes.Count; i++)
            {
                if (Math.Sign(FilmRecipes[i].Calibration) < 0)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.FilmRecipes.Errors.CalibrationIsNegative"), i));
                }
                if (Math.Sign(FilmRecipes[i].CoolingLip) < 0)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.FilmRecipes.Errors.CoolingLipIsNegative"), i));
                }
                if (FilmRecipes[i].FilmType == null)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.FilmRecipes.Errors.FilmTypeIsNull"), i));
                }
                if (Math.Sign(FilmRecipes[i].MaterialCost) < 0)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.FilmRecipes.Errors.MaterialCostIsNegative"), i));
                }
                if (FilmRecipes[i].Name == null || FilmRecipes[i].Name.Length == 0)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.FilmRecipes.Errors.NameIsNull"), i));
                }
                if (Math.Sign(FilmRecipes[i].Nozzle) < 0)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.FilmRecipes.Errors.NozzleIsNegative"), i));
                }
                if (Math.Sign(FilmRecipes[i].ProductionSpeed) < 0)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.FilmRecipes.Errors.ProductionSpeedIsNegative"), i));
                }
                if (Math.Sign(FilmRecipes[i].Thickness) < 0)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.FilmRecipes.Errors.ThicknessIsNegative"), i));
                }
            }
            return sb.ToString();
        }
    }
}
