using EasyLocalization.Localization;
using OPTEL.Data;
using OPTEL.UI.Desktop.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace OPTEL.UI.Desktop.ViewModels
{
    public class ExtruderCalibrationsViewModel : INotifyPropertyChanged
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
        public bool IsDataChanged { get => _isDataChanged; set => _isDataChanged = value; }
        #endregion
        #region Fields
        private CalibrationChange _selectedExtruderCalibration;
        private bool _isDataChanged;
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
        private RelayCommand _checkForUnsavedChangesOnWindowClosing;
        public RelayCommand CheckForUnsavedChangesOnWindowClosing
        {
            get
            {
                return _checkForUnsavedChangesOnWindowClosing ??= new RelayCommand(obj =>
                {
                    if (IsDataChanged == false)
                    {
                        return;
                    }

                });
            }
        }
        private RelayCommand _markEntityDataAsChanged;
        public RelayCommand MarkEntityDataAsChanged
        {
            get
            {
                return _markEntityDataAsChanged ??= new RelayCommand(obj =>
                {
                    IsDataChanged = true;
                });
            }
        }
        #endregion
        public ExtruderCalibrationsViewModel()
        {
            ExtruderCalibrations = Database.instance.CalibrationChangeRepository.GetAll();
            Extruders = Database.instance.ProductionLineRepository.GetAll();
            _isDataChanged = false;
        }


        #region PropertyChangedEventHandler
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
