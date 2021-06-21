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
    public class ExtruderCalibrationsViewModel : DatabaseEntityViewModel
    {
        #region Properties
        public CalibrationChange SelectedExtruderCalibration
        {
            get => _selectedExtruderCalibration;
            set
            {
                _selectedExtruderCalibration = value;
                IgnoreMarkDataChangedRequestsCommand.Execute(null);
                OnPropertyChanged("SelectedExtruderCalibration");
                AcceptMarkDataChangedRequestsCommand.Execute(null);
            }
        }
        public ObservableCollection<CalibrationChange> ExtruderCalibrations { get; set; }
        public IEnumerable<ProductionLine> Extruders { get; set; }
        #endregion
        #region Fields
        private CalibrationChange _selectedExtruderCalibration;
        #endregion
        public ExtruderCalibrationsViewModel(IWindowCloseService windowCloseService, IErrorsListWindowService errorsListService) : base(windowCloseService, errorsListService)
        {
            ExtruderCalibrations = new ObservableCollection<CalibrationChange>(Database.instance.CalibrationChangeRepository.GetAll());
            Extruders = Database.instance.ProductionLineRepository.GetAll();
        }

        public override ObservableCollection<Error> GetCustomErrors()
        {
            ObservableCollection<Error> errors = new ObservableCollection<Error>();
            int entryIndex = 0;
            for (int i = 0; i < ExtruderCalibrations.Count; i++)
            {
                entryIndex = i + 1;
                if (ExtruderCalibrations[i].ParentProductionLine == null)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.ExtruderCalibrations.Errors.TargetExtruderIsNull"), entryIndex)
                    });
                }
                if (Math.Sign(ExtruderCalibrations[i].ReconfigurationTime) < 0)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.ExtruderCalibrations.Errors.ReconfigurationTimeIsNegative"), entryIndex)
                    });
                }
                if (Math.Sign(ExtruderCalibrations[i].CalibrationToChange) < 0)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.ExtruderCalibrations.Errors.CalibrationToChangeIsNegative"), entryIndex)
                    });
                }
                if (Math.Sign(ExtruderCalibrations[i].Consumption) < 0)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.ExtruderCalibrations.Errors.ConsumptionIsNegative"), entryIndex)
                    });
                }
            }
            return errors;
        }

        public override void SelectFirstDataEntryIfExist()
        {
            CalibrationChange calibrationChange;
            try
            {
                calibrationChange = ExtruderCalibrations.First();
            }
            catch
            {
                return;
            }
            SelectedExtruderCalibration = calibrationChange;
        }

        public override void AddEntity()
        {
            CalibrationChange calibration = new CalibrationChange();
            ExtruderCalibrations.Add(calibration);
            Database.instance.CalibrationChangeRepository.Add(calibration);
            SelectedExtruderCalibration = calibration;
        }

        public override void RemoveEntity()
        {
            Database.instance.CalibrationChangeRepository.Delete(SelectedExtruderCalibration);
            ExtruderCalibrations.Remove(SelectedExtruderCalibration);
            SelectFirstDataEntryIfExistsCommand.Execute(null);
        }

        public override bool RemoveEntityExecuteCondition()
        {
            return SelectedExtruderCalibration != null;
        }

        public override void CloneEntity()
        {
            CalibrationChange calibration = new CalibrationChange();
            calibration.CalibrationToChange = SelectedExtruderCalibration.CalibrationToChange;
            calibration.ParentProductionLine = SelectedExtruderCalibration.ParentProductionLine;
            calibration.ParentProductionLineID = SelectedExtruderCalibration.ParentProductionLineID;
            calibration.ReconfigurationTime = SelectedExtruderCalibration.ReconfigurationTime;
            ExtruderCalibrations.Add(calibration);
            Database.instance.CalibrationChangeRepository.Add(calibration);
            SelectedExtruderCalibration = calibration;
        }

        public override bool CloneEntityExecuteCondition()
        {
            return SelectedExtruderCalibration != null;
        }
    }
}
