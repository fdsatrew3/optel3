using OPTEL.Data;
using OPTEL.UI.Desktop.Helpers;
using OPTEL.UI.Desktop.ViewModels.Core;
using System;
using System.Collections.Generic;
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
                OnPropertyChanged("SelectedExtruderCalibration");
            }
        }
        public IEnumerable<CalibrationChange> ExtruderCalibrations { get; set; }
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
        #endregion
        public ExtruderCalibrationsViewModel()
        {
            ExtruderCalibrations = Database.instance.CalibrationChangeRepository.GetAll();
            Extruders = Database.instance.ProductionLineRepository.GetAll();
        }
    }
}
