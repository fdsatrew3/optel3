using EasyLocalization.Localization;
using OPTEL.Data;
using OPTEL.UI.Desktop.Models;
using OPTEL.UI.Desktop.Services.ErrorsListWindows.Base;
using OPTEL.UI.Desktop.Services.WindowClosers.Base;
using OPTEL.UI.Desktop.ViewModels.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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
                IgnoreMarkDataChangedRequestsCommand.Execute(null);
                OnPropertyChanged("SelectedFilmRecipe");
                AcceptMarkDataChangedRequestsCommand.Execute(null);
            }
        }
        public ObservableCollection<FilmRecipe> FilmRecipes { get; set; }
        public IEnumerable<FilmType> FilmTypes { get; set; }
        #endregion
        #region Fields
        private FilmRecipe _SelectedFilmRecipe;
        #endregion
        public FilmRecipesViewModel(IWindowCloseService windowCloseService, IErrorsListWindowService errorsListService) : base(windowCloseService, errorsListService)
        {
            FilmRecipes = new ObservableCollection<FilmRecipe>(Database.instance.FilmRecipeRepository.GetAll());
            FilmTypes = Database.instance.FilmTypeRepository.GetAll();
        }

        public override ObservableCollection<Error> GetCustomErrors()
        {
            ObservableCollection<Error> errors = new ObservableCollection<Error>();
            int entryIndex = 0;
            for (int i = 0; i < FilmRecipes.Count; i++)
            {
                entryIndex = i + 1;
                if (Math.Sign(FilmRecipes[i].Calibration) < 0)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.FilmRecipes.Errors.CalibrationIsNegative"), entryIndex)
                    });
                }
                if (Math.Sign(FilmRecipes[i].CoolingLip) < 0)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.FilmRecipes.Errors.CoolingLipIsNegative"), entryIndex)
                    });
                }
                if (FilmRecipes[i].FilmType == null)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.FilmRecipes.Errors.FilmTypeIsNull"), entryIndex)
                    });
                }
                if (Math.Sign(FilmRecipes[i].MaterialCost) < 0)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.FilmRecipes.Errors.MaterialCostIsNegative"), entryIndex)
                    });
                }
                if (FilmRecipes[i].Name == null || FilmRecipes[i].Name.Length == 0)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.FilmRecipes.Errors.NameIsNull"), entryIndex)
                    });
                }
                if (Math.Sign(FilmRecipes[i].Nozzle) < 0)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.FilmRecipes.Errors.NozzleIsNegative"), entryIndex)
                    });
                }
                if (Math.Sign(FilmRecipes[i].ProductionSpeed) < 0)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.FilmRecipes.Errors.ProductionSpeedIsNegative"), entryIndex)
                    });
                }
                if (Math.Sign(FilmRecipes[i].Thickness) < 0)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.FilmRecipes.Errors.ThicknessIsNegative"), entryIndex)
                    });
                }
            }
            return errors;
        }

        public override void SelectFirstDataEntryIfExist()
        {
            FilmRecipe filmRecipe;
            try
            {
                filmRecipe = FilmRecipes.First();
            }
            catch
            {
                return;
            }
            SelectedFilmRecipe = filmRecipe;
        }

        public override void AddEntity()
        {
            FilmRecipe recipe = new FilmRecipe();
            FilmRecipes.Add(recipe);
            Database.instance.FilmRecipeRepository.Add(recipe);
            SelectedFilmRecipe = recipe;
        }

        public override void RemoveEntity()
        {
            Database.instance.FilmRecipeRepository.Delete(SelectedFilmRecipe);
            FilmRecipes.Remove(SelectedFilmRecipe);
            SelectFirstDataEntryIfExistsCommand.Execute(null);
        }

        public override bool RemoveEntityExecuteCondition()
        {
            return SelectedFilmRecipe != null;
        }

        public override void CloneEntity()
        {
            FilmRecipe recipe = new FilmRecipe();
            recipe.Calibration = SelectedFilmRecipe.Calibration;
            recipe.CoolingLip = SelectedFilmRecipe.CoolingLip;
            recipe.FilmType = SelectedFilmRecipe.FilmType;
            recipe.FilmTypeID = SelectedFilmRecipe.FilmTypeID;
            recipe.MaterialCost = SelectedFilmRecipe.MaterialCost;
            recipe.Name = SelectedFilmRecipe.Name;
            recipe.Nozzle = SelectedFilmRecipe.Nozzle;
            recipe.ProductionSpeed = SelectedFilmRecipe.ProductionSpeed;
            recipe.Thickness = SelectedFilmRecipe.Thickness;
            FilmRecipes.Add(recipe);
            Database.instance.FilmRecipeRepository.Add(recipe);
            SelectedFilmRecipe = recipe;
        }

        public override bool CloneEntityExecuteCondition()
        {
            return SelectedFilmRecipe != null;
        }
    }
}
