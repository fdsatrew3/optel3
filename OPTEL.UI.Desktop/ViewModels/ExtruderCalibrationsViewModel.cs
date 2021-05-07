using EasyLocalization.Localization;
using OPTEL.Data;
using OPTEL.UI.Desktop.Helpers;
using OPTEL.UI.Desktop.ViewModels.Core;
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
            }
        }
        public ObservableCollection<CalibrationChange> ExtruderCalibrations { get; set; }
        public IEnumerable<ProductionLine> Extruders { get; set; }
        #endregion
        #region Fields
        private CalibrationChange _selectedExtruderCalibration;

        private RelayCommand _selectFirstDataEntryIfExists;
        private RelayCommand _addEntity;
        private RelayCommand _removeEntity;
        private RelayCommand _cloneEntity;
        #endregion
        #region Commands
        public RelayCommand SelectFirstDataEntryIfExists
        {
            get
            {
                return _selectFirstDataEntryIfExists ??= new RelayCommand(obj =>
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
        public RelayCommand AddEntity
        {
            get
            {
                return _addEntity ??= new RelayCommand(obj =>
                {
                    CalibrationChange calibration = new CalibrationChange();
                    ExtruderCalibrations.Add(calibration);
                    Database.instance.CalibrationChangeRepository.Add(calibration);
                    SelectedExtruderCalibration = calibration;
                });
            }
        }
        public RelayCommand RemoveEntity
        {
            get
            {
                return _removeEntity ??= new RelayCommand(obj =>
                {
                    Database.instance.CalibrationChangeRepository.Delete(SelectedExtruderCalibration);
                    ExtruderCalibrations.Remove(SelectedExtruderCalibration);
                    SelectFirstDataEntryIfExists.Execute(null);
                }, (obj) => SelectedExtruderCalibration != null);
            }
        }
        public RelayCommand CloneEntity
        {
            get
            {
                return _cloneEntity ??= new RelayCommand(obj =>
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

        public override string CheckForCustomErrors()
        {
            string result = string.Empty;
            StringBuilder sb = new StringBuilder(result);
            for (int i = 0; i < ExtruderCalibrations.Count; i++)
            {
                if (ExtruderCalibrations[i].ParentProductionLine == null)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.ExtruderCalibrations.Errors.TargetExtruderIsNull"), i));
                }
            }
            return sb.ToString();
        }
        public ExtruderCalibrationsViewModel()
        {
            ExtruderCalibrations = new ObservableCollection<CalibrationChange>(Database.instance.CalibrationChangeRepository.GetAll());
            Extruders = Database.instance.ProductionLineRepository.GetAll();
        }
    }
}
