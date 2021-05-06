using OPTEL.UI.Desktop.Helpers;
using OPTEL.UI.Desktop.Models;
using OPTEL.UI.Desktop.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OPTEL.UI.Desktop.ViewModels
{
    public class ExtruderCalibrationsViewModel
    {
        private ExtruderCalibration _selectedExtruderCalibration;

        public ObservableCollection<ExtruderCalibration> Phones { get; set; }

        public ExtruderCalibration SelectedExtruderCalibration
        {
            get { return _selectedExtruderCalibration; }
            set
            {
                _selectedExtruderCalibration = value;
                OnPropertyChanged("SelectedExtruderCalibration");
            }
        }

        public ExtruderCalibrationsViewModel()
        {

        }
        #region PropertyChangedEventHandler
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion
    }
}
