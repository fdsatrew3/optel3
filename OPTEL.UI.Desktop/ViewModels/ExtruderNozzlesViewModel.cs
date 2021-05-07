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
    public class ExtruderNozzlesViewModel : DatabaseEntityViewModel
    {
        #region Properties
        public NozzleChange SelectedExtruderNozzle
        {
            get => _selectedExtruderNozzle;
            set
            {
                _selectedExtruderNozzle = value;
                OnPropertyChanged("SelectedExtruderNozzle");
            }
        }
        public ObservableCollection<NozzleChange> ExtruderNozzles { get; set; }
        public IEnumerable<ProductionLine> Extruders { get; set; }
        #endregion
        #region Fields
        private NozzleChange _selectedExtruderNozzle;

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
                    NozzleChange nozzleChange;
                    try
                    {
                        nozzleChange = ExtruderNozzles.First();
                    }
                    catch
                    {
                        return;
                    }
                    SelectedExtruderNozzle = nozzleChange;
                });
            }
        }
        public RelayCommand AddEntity
        {
            get
            {
                return _addEntity ??= new RelayCommand(obj =>
                {
                    NozzleChange nozzle = new NozzleChange();
                    ExtruderNozzles.Add(nozzle);
                    Database.instance.NozzleChangeRepository.Add(nozzle);
                    SelectedExtruderNozzle = nozzle;
                });
            }
        }
        public RelayCommand RemoveEntity
        {
            get
            {
                return _removeEntity ??= new RelayCommand(obj =>
                {
                    Database.instance.NozzleChangeRepository.Delete(SelectedExtruderNozzle);
                    ExtruderNozzles.Remove(SelectedExtruderNozzle);
                    SelectFirstDataEntryIfExists.Execute(null);
                }, (obj) => SelectedExtruderNozzle != null);
            }
        }
        public RelayCommand CloneEntity
        {
            get
            {
                return _cloneEntity ??= new RelayCommand(obj =>
                {
                    NozzleChange nozzle = new NozzleChange();
                    nozzle.NozzleToChange = SelectedExtruderNozzle.NozzleToChange;
                    nozzle.ParentProductionLine = SelectedExtruderNozzle.ParentProductionLine;
                    nozzle.ParentProductionLineID = SelectedExtruderNozzle.ParentProductionLineID;
                    nozzle.ReconfigurationTime = SelectedExtruderNozzle.ReconfigurationTime;
                    ExtruderNozzles.Add(nozzle);
                    Database.instance.NozzleChangeRepository.Add(nozzle);
                    SelectedExtruderNozzle = nozzle;
                }, (obj) => SelectedExtruderNozzle != null);
            }
        }
        #endregion

        public override string CheckForCustomErrors()
        {
            string result = string.Empty;
            StringBuilder sb = new StringBuilder(result);
            for (int i = 0; i < ExtruderNozzles.Count; i++)
            {
                if (ExtruderNozzles[i].ParentProductionLine == null)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.ExtruderNozzles.Errors.TargetExtruderIsNull"), i));
                }
            }
            return sb.ToString();
        }
        public ExtruderNozzlesViewModel()
        {
            ExtruderNozzles = new ObservableCollection<NozzleChange>(Database.instance.NozzleChangeRepository.GetAll());
            Extruders = Database.instance.ProductionLineRepository.GetAll();
        }
    }
}
