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
    public class ExtruderCalibrationsViewModel : DatabaseEntityViewModel
    {
        #region Properties
        public CalibrationChange SelectedExtruderCalibration
        {
            get => _selectedExtruderCalibration;
            set
            {
                _selectedExtruderCalibration = value;
                OnPropertyChanged("SelectedExtruderCalibration");
                IsDataChanged = false;
            }
        }
        public ObservableCollection<CalibrationChange> ExtruderCalibrations { get; set; }
        public IEnumerable<ProductionLine> Extruders { get; set; }
        #endregion
        #region Fields
        private CalibrationChange _selectedExtruderCalibration;

        private RelayCommand _selectFirstDataEntryIfExistsCommand;
        private RelayCommand _addEntityCommand;
        private RelayCommand _removeEntityCommand;
        private RelayCommand _cloneEntityCommand;
        #endregion
        public ExtruderCalibrationsViewModel(IDatabaseEntityWindowCloseService windowCloseService, IErrorsListWindowService errorsListService) : base(windowCloseService, errorsListService)
        {
            ExtruderCalibrations = new ObservableCollection<CalibrationChange>(Database.instance.CalibrationChangeRepository.GetAll());
            Extruders = Database.instance.ProductionLineRepository.GetAll();
        }

        #region Commands
        public RelayCommand SelectFirstDataEntryIfExistsCommand
        {
            get
            {
                return _selectFirstDataEntryIfExistsCommand ??= new RelayCommand(obj =>
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
                });
            }
        }
        public RelayCommand AddEntityCommand
        {
            get
            {
                return _addEntityCommand ??= new RelayCommand(obj =>
                {
                    CalibrationChange calibration = new CalibrationChange();
                    ExtruderCalibrations.Add(calibration);
                    Database.instance.CalibrationChangeRepository.Add(calibration);
                    SelectedExtruderCalibration = calibration;
                });
            }
        }
        public RelayCommand RemoveEntityCommand
        {
            get
            {
                return _removeEntityCommand ??= new RelayCommand(obj =>
                {
                    Database.instance.CalibrationChangeRepository.Delete(SelectedExtruderCalibration);
                    ExtruderCalibrations.Remove(SelectedExtruderCalibration);
                    SelectFirstDataEntryIfExistsCommand.Execute(null);
                }, (obj) => SelectedExtruderCalibration != null);
            }
        }
        public RelayCommand CloneEntityCommand
        {
            get
            {
                return _cloneEntityCommand ??= new RelayCommand(obj =>
                {
                    CalibrationChange calibration = new CalibrationChange();
                    calibration.CalibrationToChange = SelectedExtruderCalibration.CalibrationToChange;
                    calibration.ParentProductionLine = SelectedExtruderCalibration.ParentProductionLine;
                    calibration.ParentProductionLineID = SelectedExtruderCalibration.ParentProductionLineID;
                    calibration.ReconfigurationTime = SelectedExtruderCalibration.ReconfigurationTime;
                    ExtruderCalibrations.Add(calibration);
                    Database.instance.CalibrationChangeRepository.Add(calibration);
                    SelectedExtruderCalibration = calibration;
                }, (obj) => SelectedExtruderCalibration != null);
            }
        }
        #endregion

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
            }
            return errors;
        }
    }
}
