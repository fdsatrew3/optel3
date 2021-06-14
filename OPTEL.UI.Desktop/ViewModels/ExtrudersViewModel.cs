using EasyLocalization.Localization;
using OPTEL.Data;
using OPTEL.UI.Desktop.Models;
using OPTEL.UI.Desktop.Services.ErrorsListWindows.Base;
using OPTEL.UI.Desktop.Services.WindowClosers.Base;
using OPTEL.UI.Desktop.ViewModels.Core;
using System;
using System.Collections.ObjectModel;
using System.Linq;

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
                IgnoreMarkDataChangedRequestsCommand.Execute(null);
                OnPropertyChanged("SelectedExtruder");
                AcceptMarkDataChangedRequestsCommand.Execute(null);
            }
        }
        public ObservableCollection<ProductionLine> Extruders { get; set; }
        #endregion
        #region Fields
        private ProductionLine _SelectedExtruder;
        #endregion
        public ExtrudersViewModel(IWindowCloseService windowCloseService, IErrorsListWindowService errorsListService) : base(windowCloseService, errorsListService)
        {
            Extruders = new ObservableCollection<ProductionLine>(Database.instance.ProductionLineRepository.GetAll());
        }
        public override ObservableCollection<Error> GetCustomErrors()
        {
            ObservableCollection<Error> errors = new ObservableCollection<Error>();
            int entryIndex = 0;
            for (int i = 0; i < Extruders.Count; i++)
            {
                entryIndex = i + 1;
                if (Extruders[i].Name == null || Extruders[i].Name.Length == 0)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.NameIsNull"), entryIndex)
                    });
                }
                if (Extruders[i].Code == null || Extruders[i].Code.Length == 0)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.CodeIsNull"), entryIndex)
                    });
                }
                if (Math.Sign(Extruders[i].HourCost) < 0)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.HourCostIsNegative"), entryIndex)
                    });
                }
                if (Math.Sign(Extruders[i].MaxProductionSpeed) < 0)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.MaxProductionSpeedIsNegative"), entryIndex)
                    });
                }
                if (Math.Sign(Extruders[i].WidthMin) < 0)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.WidthMinIsNegative"), entryIndex)
                    });
                }
                if (Math.Sign(Extruders[i].WidthMax) < 0)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.WidthMaxIsNegative"), entryIndex)
                    });
                }
                if (Extruders[i].WidthMin > Extruders[i].WidthMax)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.WidthMinIsGreaterThanWidthMax"), entryIndex)
                    });
                }
                if (Math.Sign(Extruders[i].ThicknessMin) < 0)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.ThicknessMinIsNegative"), entryIndex)
                    });
                }
                if (Math.Sign(Extruders[i].ThicknessMax) < 0)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.ThicknessMaxIsNegative"), entryIndex)
                    });
                }
                if (Extruders[i].ThicknessMin > Extruders[i].ThicknessMax)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.ThicknessMinIsGreaterThanThicknessMax"), entryIndex)
                    });
                }
                if (Math.Sign(Extruders[i].WeightMin) < 0)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.WeightMinIsNegative"), entryIndex)
                    });
                }
                if (Math.Sign(Extruders[i].WeightMax) < 0)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.WeightMaxIsNegative"), entryIndex)
                    });
                }
                if (Extruders[i].WeightMin > Extruders[i].WeightMax)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.WeightMinIsGreaterThanWeightMax"), entryIndex)
                    });
                }
                if (Math.Sign(Extruders[i].LengthMin) < 0)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.LengthMinIsNegative"), entryIndex)
                    });
                }
                if (Math.Sign(Extruders[i].LengthMax) < 0)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.LengthMaxIsNegative"), entryIndex)
                    });
                }
                if (Extruders[i].LengthMin > Extruders[i].LengthMax)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.LengthMinIsGreaterThanLengthMax"), entryIndex)
                    });
                }
                if (Math.Sign(Extruders[i].WidthChangeTime) < 0)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.WidthChangeTimeIsNegative"), entryIndex)
                    });
                }
                if (Math.Sign(Extruders[i].ThicknessChangeTime) < 0)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.ThicknessChangeTimeIsNegative"), entryIndex)
                    });
                }
                if (Math.Sign(Extruders[i].WidthChangeConsumption) < 0)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.WidthChangeConsumptionIsNegative"), entryIndex)
                    });
                }
                if (Math.Sign(Extruders[i].ThicknessChangeConsumption) < 0)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.Extruders.Errors.ThicknessChangeConsumptionIsNegative"), entryIndex)
                    });
                }
            }
            return errors;
        }

        public override void SelectFirstDataEntryIfExist()
        {
            ProductionLine productionLine;
            try
            {
                productionLine = Extruders.First();
            }
            catch
            {
                return;
            }
            SelectedExtruder = productionLine;
        }

        public override void AddEntity()
        {
            ProductionLine productionLine = new ProductionLine();
            Extruders.Add(productionLine);
            Database.instance.ProductionLineRepository.Add(productionLine);
            SelectedExtruder = productionLine;
        }

        public override void RemoveEntity()
        {
            Database.instance.ProductionLineRepository.Delete(SelectedExtruder);
            Extruders.Remove(SelectedExtruder);
            SelectFirstDataEntryIfExistsCommand.Execute(null);
        }

        public override bool RemoveEntityExecuteCondition()
        {
            return SelectedExtruder != null;
        }

        public override void CloneEntity()
        {
            ProductionLine productionLine = new ProductionLine();
            productionLine.CalibrationChanges = SelectedExtruder.CalibrationChanges;
            productionLine.Code = SelectedExtruder.Code;
            productionLine.CoolingLipChanges = SelectedExtruder.CoolingLipChanges;
            productionLine.FilmTypesChanges = SelectedExtruder.FilmTypesChanges;
            productionLine.HourCost = SelectedExtruder.HourCost;
            productionLine.LengthMax = SelectedExtruder.LengthMax;
            productionLine.LengthMin = SelectedExtruder.LengthMin;
            productionLine.MaxProductionSpeed = SelectedExtruder.MaxProductionSpeed;
            productionLine.Name = SelectedExtruder.Name;
            productionLine.NozzleChanges = SelectedExtruder.NozzleChanges;
            productionLine.ThicknessChangeTime = SelectedExtruder.ThicknessChangeTime;
            productionLine.ThicknessMax = SelectedExtruder.ThicknessMax;
            productionLine.ThicknessMin = SelectedExtruder.ThicknessMin;
            productionLine.WeightMax = SelectedExtruder.WeightMax;
            productionLine.WeightMin = SelectedExtruder.WeightMin;
            productionLine.WidthChangeTime = SelectedExtruder.WidthChangeTime;
            productionLine.WidthMax = SelectedExtruder.WidthMax;
            productionLine.WidthMin = SelectedExtruder.WidthMin;
            Extruders.Add(productionLine);
            Database.instance.ProductionLineRepository.Add(productionLine);
            SelectedExtruder = productionLine;
        }

        public override bool CloneEntityExecuteCondition()
        {
            return SelectedExtruder != null;
        }
    }
}
