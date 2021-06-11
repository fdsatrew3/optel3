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
    public class ExtruderRecipesChangeViewModel : DatabaseEntityViewModel
    {
        #region Properties
        public FilmTypesChange SelectedExtruderRecipe
        {
            get => _SelectedExtruderRecipe;
            set
            {
                _SelectedExtruderRecipe = value;
                OnPropertyChanged("SelectedExtruderRecipe");
                IsDataChanged = false;
            }
        }
        public ObservableCollection<FilmTypesChange> ExtruderRecipes { get; set; }
        public IEnumerable<ProductionLine> Extruders { get; set; }
        public IEnumerable<FilmType> FilmTypes { get; set; }
        #endregion
        #region Fields
        private FilmTypesChange _SelectedExtruderRecipe;

        private RelayCommand _addEntityCommand;
        private RelayCommand _removeEntityCommand;
        private RelayCommand _cloneEntityCommand;
        #endregion
        public ExtruderRecipesChangeViewModel(IWindowCloseService windowCloseService, IErrorsListWindowService errorsListService) : base(windowCloseService, errorsListService)
        {
            ExtruderRecipes = new ObservableCollection<FilmTypesChange>(Database.instance.FilmRecipeChangeRepository.GetAll());
            Extruders = Database.instance.ProductionLineRepository.GetAll();
            FilmTypes = Database.instance.FilmTypeRepository.GetAll();
        }

        #region Commands
        public RelayCommand AddEntityCommand
        {
            get
            {
                return _addEntityCommand ??= new RelayCommand(obj =>
                {
                    FilmTypesChange change = new FilmTypesChange();
                    ExtruderRecipes.Add(change);
                    Database.instance.FilmRecipeChangeRepository.Add(change);
                    SelectedExtruderRecipe = change;
                });
            }
        }
        public RelayCommand RemoveEntityCommand
        {
            get
            {
                return _removeEntityCommand ??= new RelayCommand(obj =>
                {
                    Database.instance.FilmRecipeChangeRepository.Delete(SelectedExtruderRecipe);
                    ExtruderRecipes.Remove(SelectedExtruderRecipe);
                    SelectFirstDataEntryIfExistsCommand.Execute(null);
                }, (obj) => SelectedExtruderRecipe != null);
            }
        }
        public RelayCommand CloneEntityCommand
        {
            get
            {
                return _cloneEntityCommand ??= new RelayCommand(obj =>
                {
                    FilmTypesChange change = new FilmTypesChange();
                    change.FilmTypeFrom = SelectedExtruderRecipe.FilmTypeFrom;
                    change.FilmTypeTo = SelectedExtruderRecipe.FilmTypeTo;
                    change.ParentProductionLine = SelectedExtruderRecipe.ParentProductionLine;
                    change.ParentProductionLineID = SelectedExtruderRecipe.ParentProductionLineID;
                    change.ReconfigurationTime = SelectedExtruderRecipe.ReconfigurationTime;
                    ExtruderRecipes.Add(change);
                    Database.instance.FilmRecipeChangeRepository.Add(change);
                    SelectedExtruderRecipe = change;
                }, (obj) => SelectedExtruderRecipe != null);
            }
        }
        #endregion

        public override ObservableCollection<Error> GetCustomErrors()
        {
            ObservableCollection<Error> errors = new ObservableCollection<Error>();
            int entryIndex = 0;
            for (int i = 0; i < ExtruderRecipes.Count; i++)
            {
                entryIndex = i + 1;
                if (ExtruderRecipes[i].ParentProductionLine == null)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.ExtruderRecipesChange.Errors.TargetExtruderIsNull"), entryIndex)
                    });
                }
                if (ExtruderRecipes[i].FilmTypeTo == ExtruderRecipes[i].FilmTypeFrom)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.ExtruderRecipesChange.Errors.FilmTypeToEqualToFilmTypeFrom"), entryIndex)
                    });
                }
                if (ExtruderRecipes[i].FilmTypeTo == null)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.ExtruderRecipesChange.Errors.FilmTypeToIsNull"), entryIndex)
                    });
                }
                if (ExtruderRecipes[i].FilmTypeFrom == null)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.ExtruderRecipesChange.Errors.FilmTypeFromIsNull"), entryIndex)
                    });
                }
                if (Math.Sign(ExtruderRecipes[i].ReconfigurationTime) < 0)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.ExtruderRecipesChange.Errors.ReconfigurationTimeIsNegative"), entryIndex)
                    });
                }
            }
            return errors;
        }

        public override void SelectFirstDataEntryIfExist()
        {
            FilmTypesChange extruderRecipe;
            try
            {
                extruderRecipe = ExtruderRecipes.First();
            }
            catch
            {
                return;
            }
            SelectedExtruderRecipe = extruderRecipe;
        }
    }
}
