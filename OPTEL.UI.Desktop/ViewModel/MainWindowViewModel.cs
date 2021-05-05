using OPTEL.UI.Desktop.Helpers;
using OPTEL.UI.Desktop.View;
using System;

namespace OPTEL.UI.Desktop.ViewModel
{
    public class MainWindowViewModel
    {
        #region Fields
        private RelayCommand _openExtruderCalibrationsWindowCommand;
        #endregion

        public RelayCommand OpenExtruderCalibrationsWindowCommand
        {
            get
            {
                return _openExtruderCalibrationsWindowCommand ??= new RelayCommand(obj =>
                                  {
                                      ExtruderCalibrationsWindow window = new ExtruderCalibrationsWindow();
                                      window.ShowModalDialog();
                                  });
            }
        }
    }
}
