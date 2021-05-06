using OPTEL.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OPTEL.UI.Desktop.ViewModels
{
    public class ExtruderCalibrationsViewModel
    {
        private CalibrationChange _selectedExtruderCalibration;

        public CalibrationChange SelectedExtruderCalibration { get => _selectedExtruderCalibration; set => _selectedExtruderCalibration = value; }

        public IEnumerable<CalibrationChange> ExtruderCalibrations { get; set; }

        public ExtruderCalibrationsViewModel()
        {
            ExtruderCalibrations = Database.instance.CalibrationChangeRepository.GetAll();
        }
    }
}
