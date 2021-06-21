using Microsoft.Win32;
using OPTEL.UI.Desktop.Services.OpenFileDialogs.Base;
using System.Windows;

namespace OPTEL.UI.Desktop.Services.OpenFileDialogs
{
    public class ExcelOpenFileDialogService : IOpenFileDialogService
    {
        private Window _parent;
        public ExcelOpenFileDialogService(Window parent)
        {
            _parent = parent;
        }
        public string ShowOpenFileDialog()
        {
            string result = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files |*.xls;*.xlsx;*.xlsm";
            if (openFileDialog.ShowDialog(_parent) == true)
            {
                result = openFileDialog.FileName;
            }
            return result;
        }
    }
}
