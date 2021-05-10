using EasyLocalization.Localization;
using OPTEL.Data;
using OPTEL.UI.Desktop.Helpers;
using OPTEL.UI.Desktop.Services.ErrorsListWindows.Base;
using OPTEL.UI.Desktop.Services.WindowClosers.Base;
using OPTEL.UI.Desktop.ViewModels.Core;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace OPTEL.UI.Desktop.ViewModels
{
    public class ExtrudersViewModel : DatabaseEntityViewModel
    {
        #region Properties
        public ProductionLine SelectedExtruder
        {
            get => _SelectedExtruder;
            set
            {
                _SelectedExtruder = value;
                OnPropertyChanged("SelectedExtruder");
                IsDataChanged = false;
            }
        }
        public ObservableCollection<ProductionLine> Extruders { get; set; }
        #endregion
        #region Fields
        private ProductionLine _SelectedExtruder;

        private RelayCommand _selectFirstDataEntryIfExistsCommand;
        private RelayCommand _addEntityCommand;
        private RelayCommand _removeEntityCommand;
        private RelayCommand _cloneEntityCommand;
        #endregion
        public ExtrudersViewModel(IDatabaseEntityWindowCloseService windowCloseService, IErrorsListWindowService errorsListService) : base(windowCloseService, errorsListService)
        {
            Extruders = new ObservableCollection<ProductionLine>(Database.instance.ProductionLineRepository.GetAll());
        }

        #region Commands
        public RelayCommand SelectFirstDataEntryIfExistsCommand
        {
            get
            {
                return _selectFirstDataEntryIfExistsCommand ??= new RelayCommand(obj =>
                {
                    ProductionLine extruder;
                    try
                    {
                        extruder = Extruders.First();
                    }
                    catch
                    {
                        return;
                    }
                    SelectedExtruder = extruder;
                });
            }
        }
        public RelayCommand AddEntityCommand
        {
            get
            {
                return _addEntityCommand ??= new RelayCommand(obj =>
                {
                    ProductionLine change = new ProductionLine();
                    Extruders.Add(change);
                    Database.instance.ProductionLineRepository.Add(change);
                    SelectedExtruder = change;
                });
            }
        }
        public RelayCommand RemoveEntityCommand
        {
            get
            {
                return _removeEntityCommand ??= new RelayCommand(obj =>
                {
                    Database.instance.ProductionLineRepository.Delete(SelectedExtruder);
                    Extruders.Remove(SelectedExtruder);
                    SelectFirstDataEntryIfExistsCommand.Execute(null);
                }, (obj) => SelectedExtruder != null);
            }
        }
        public RelayCommand CloneEntityCommand
        {
            get
            {
                return _cloneEntityCommand ??= new RelayCommand(obj =>
                {
                    ProductionLine change = new ProductionLine();
                    change.CalibrationChanges = SelectedExtruder.CalibrationChanges;
                    change.Code = SelectedExtruder.Code;
                    change.CoolingLipChanges = SelectedExtruder.CoolingLipChanges;
                    change.FilmTypesChanges = SelectedExtruder.FilmTypesChanges;
                    change.HourCost = SelectedExtruder.HourCost;
                    change.LengthMax = SelectedExtruder.LengthMax;
                    change.LengthMin = SelectedExtruder.LengthMin;
                    change.MaxProductionSpeed = SelectedExtruder.MaxProductionSpeed;
                    change.Name = SelectedExtruder.Name;
                    change.NozzleChanges = SelectedExtruder.NozzleChanges;
                    change.ThicknessChangeTime = SelectedExtruder.ThicknessChangeTime;
                    change.ThicknessMax = SelectedExtruder.ThicknessMax;
                    change.ThicknessMin = SelectedExtruder.ThicknessMin;
                    change.WeightMax = SelectedExtruder.WeightMax;
                    change.WeightMin = SelectedExtruder.WeightMin;
                    change.WidthChangeTime = SelectedExtruder.WidthChangeTime;
                    change.WidthMax = SelectedExtruder.WidthMax;
                    change.WidthMin = SelectedExtruder.WidthMin;
                    Extruders.Add(change);
                    Database.instance.ProductionLineRepository.Add(change);
                    SelectedExtruder = change;
                }, (obj) => SelectedExtruder != null);
            }
        }
        #endregion

        public override string GetCustomErrorString()
        {
            string result = string.Empty;
            StringBuilder sb = new StringBuilder(result);
            int entryIndex = 0;
            for (int i = 0; i < Extruders.Count; i++)
            {
                entryIndex = i + 1;
                if (Extruders[i].Name == null || Extruders[i].Name.Length == 0)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.NameIsNull"), entryIndex));
                }
                if (Extruders[i].Code == null || Extruders[i].Code.Length == 0)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.CodeIsNull"), entryIndex));
                }
                if (Math.Sign(Extruders[i].HourCost) < 0)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.HourCostIsNegative"), entryIndex));
                }
                if (Math.Sign(Extruders[i].MaxProductionSpeed) < 0)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.MaxProductionSpeedIsNegative"), entryIndex));
                }
                if (Math.Sign(Extruders[i].WidthMin) < 0)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.WidthMinIsNegative"), entryIndex));
                }
                if (Math.Sign(Extruders[i].WidthMax) < 0)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.WidthMaxIsNegative"), entryIndex));
                }
                if (Extruders[i].WidthMin > Extruders[i].WidthMax)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.WidthMinIsGreaterThanWidthMax"), entryIndex));
                }
                if (Math.Sign(Extruders[i].ThicknessMin) < 0)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.ThicknessMinIsNegative"), entryIndex));
                }
                if (Math.Sign(Extruders[i].ThicknessMax) < 0)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.ThicknessMaxIsNegative"), entryIndex));
                }
                if (Extruders[i].ThicknessMin > Extruders[i].ThicknessMax)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.ThicknessMinIsGreaterThanThicknessMax"), entryIndex));
                }
                if (Math.Sign(Extruders[i].WeightMin) < 0)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.WeightMinIsNegative"), entryIndex));
                }
                if (Math.Sign(Extruders[i].WeightMax) < 0)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.WeightMaxIsNegative"), entryIndex));
                }
                if (Extruders[i].WeightMin > Extruders[i].WeightMax)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.WeightMinIsGreaterThanWeightMax"), entryIndex));
                }
                if (Math.Sign(Extruders[i].LengthMin) < 0)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.LengthMinIsNegative"), entryIndex));
                }
                if (Math.Sign(Extruders[i].LengthMax) < 0)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.LengthMaxIsNegative"), entryIndex));
                }
                if (Extruders[i].LengthMin > Extruders[i].LengthMax)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.LengthMinIsGreaterThanLengthMax"), entryIndex));
                }
                if (Math.Sign(Extruders[i].WidthChangeTime) < 0)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.WidthChangeTimeIsNegative"), entryIndex));
                }
                if (Math.Sign(Extruders[i].ThicknessChangeTime) < 0)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.ThicknessChangeTimeIsNegative"), entryIndex));
                }
            }
            return sb.ToString();
        }
    }
}
