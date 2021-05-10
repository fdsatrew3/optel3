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
            }
        }
        public ObservableCollection<FilmTypesChange> ExtruderRecipes { get; set; }
        public IEnumerable<ProductionLine> Extruders { get; set; }
        public IEnumerable<FilmType> FilmTypes { get; set; }
        #endregion
        #region Fields
        private FilmTypesChange _SelectedExtruderRecipe;

        private RelayCommand _selectFirstDataEntryIfExistsCommand;
        private RelayCommand _addEntityCommand;
        private RelayCommand _removeEntityCommand;
        private RelayCommand _cloneEntityCommand;
        #endregion
        public ExtruderRecipesChangeViewModel(IWindowCloseService windowCloseService) : base(windowCloseService)
        {
            ExtruderRecipes = new ObservableCollection<FilmTypesChange>(Database.instance.FilmRecipeChangeRepository.GetAll());
            Extruders = Database.instance.ProductionLineRepository.GetAll();
            FilmTypes = Database.instance.FilmTypeRepository.GetAll();
        }

        #region Commands
        public RelayCommand SelectFirstDataEntryIfExistsCommand
        {
            get
            {
                return _selectFirstDataEntryIfExistsCommand ??= new RelayCommand(obj =>
                {
                    FilmTypesChange coolingLip;
                    try
                    {
                        coolingLip = ExtruderRecipes.First();
                    }
                    catch
                    {
                        return;
                    }
                    SelectedExtruderRecipe = coolingLip;
                });
            }
        }
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

        public override string GetCustomErrorString()
        {
            string result = string.Empty;
            StringBuilder sb = new StringBuilder(result);
            for (int i = 0; i < ExtruderRecipes.Count; i++)
            {
                if (ExtruderRecipes[i].ParentProductionLine == null)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.ExtruderRecipesChange.Errors.TargetExtruderIsNull"), i));
                }
                if (ExtruderRecipes[i].FilmTypeTo == ExtruderRecipes[i].FilmTypeFrom)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.ExtruderRecipesChange.Errors.FilmTypeToEqualToFilmTypeFrom"), i));
                }
                if (ExtruderRecipes[i].FilmTypeTo == null)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.ExtruderRecipesChange.Errors.FilmTypeToIsNull"), i));
                }
                if (ExtruderRecipes[i].FilmTypeFrom == null)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.ExtruderRecipesChange.Errors.FilmTypeFromIsNull"), i));
                }
                if (Math.Sign(ExtruderRecipes[i].ReconfigurationTime) < 0)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.ExtruderRecipesChange.Errors.ReconfigurationTimeIsNegative"), i));
                }
            }
            return sb.ToString();
        }
    }
}
