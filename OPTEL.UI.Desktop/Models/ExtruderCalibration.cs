using OPTEL.Data;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OPTEL.UI.Desktop.Models
{
    public class ExtruderCalibration : INotifyPropertyChanged
    {
        private int _id;
        private decimal _calibrationToChange;
        private decimal _reconfigurationTime;
        private ProductionLine _productionLine;

        public int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("ID");
            }
        }
        public decimal CalibrationToChange
        {
            get { return _calibrationToChange; }
            set
            {
                _calibrationToChange = value;
                OnPropertyChanged("CalibrationToChange");
            }
        }
        public decimal ReconfigurationTime
        {
            get { return _reconfigurationTime; }
            set
            {
                _reconfigurationTime = value;
                OnPropertyChanged("ReconfigurationTime");
            }
        }
        public ProductionLine ProductionLine
        {
            get { return _productionLine; }
            set
            {
                _productionLine = value;
                OnPropertyChanged("ProductionLine");
            }
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
