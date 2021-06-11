using EasyLocalization.Localization;
using OPTEL.Data;
using OPTEL.UI.Desktop.Helpers;
using OPTEL.UI.Desktop.Models;
using OPTEL.UI.Desktop.Services.ErrorsListWindows.Base;
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
                IsDataChanged = false;
            }
        }
        public ObservableCollection<FilmRecipe> FilmRecipes { get; set; }
        public IEnumerable<FilmType> FilmTypes { get; set; }
        #endregion
        #region Fields
        private FilmRecipe _SelectedFilmRecipe;

        private RelayCommand _addEntityCommand;
        private RelayCommand _removeEntityCommand;
        private RelayCommand _cloneEntityCommand;
        #endregion
        public FilmRecipesViewModel(IWindowCloseService windowCloseService, IErrorsListWindowService errorsListService) : base(windowCloseService, errorsListService)
        {
            FilmRecipes = new ObservableCollection<FilmRecipe>(Database.instance.FilmRecipeRepository.GetAll());
            FilmTypes = Database.instance.FilmTypeRepository.GetAll();
        }

        #region Commands
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
    }
}
