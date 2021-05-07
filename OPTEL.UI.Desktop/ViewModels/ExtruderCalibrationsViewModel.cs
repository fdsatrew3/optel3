using OPTEL.Data;
using OPTEL.UI.Desktop.Helpers;
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
            }
        }
        public ObservableCollection<CalibrationChange> ExtruderCalibrations { get; set; }
        public IEnumerable<ProductionLine> Extruders { get; set; }
        #endregion
        #region Fields
        private CalibrationChange _selectedExtruderCalibration;
        #endregion
        #region Commands
        private RelayCommand _selectFirstListBoxEntryIfExists;
        public RelayCommand SelectFirstListBoxEntryIfExists
        {
            get
            {
                return _selectFirstListBoxEntryIfExists ??= new RelayCommand(obj =>
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
        private RelayCommand _addEntity;
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
        } /*
        public RelayCommand RemoveEntityCommand
        {
            get
            {
                return _removeEntityCommand ??= new RelayCommand(obj =>
                {
                    RemoveEntity();
                }, CanExecuteRemoveEntity);
            }
        }
        public RelayCommand CloneEntityCommand
        {
            get
            {
                return _cloneEntityCommand ??= new RelayCommand(obj =>
                {
                    CloneEntity();
                });
            }
        } */
        #endregion

        public override string CheckForCustomErrors()
        {
            string result = string.Empty;
            StringBuilder sb = new StringBuilder(result);
            foreach (CalibrationChange calibration in ExtruderCalibrations)
            {
                if (calibration.ParentProductionLine == null)
                {
                    sb.AppendLine("123");
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
