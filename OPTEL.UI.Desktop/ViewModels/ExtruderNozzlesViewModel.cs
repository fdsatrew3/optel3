using EasyLocalization.Localization;
using OPTEL.Data;
using OPTEL.UI.Desktop.Helpers;
using OPTEL.UI.Desktop.Services.WindowClosers.Base;
using OPTEL.UI.Desktop.ViewModels.Core;
using System;
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
                IsDataChanged = false;
            }
        }
        public ObservableCollection<NozzleChange> ExtruderNozzles { get; set; }
        public IEnumerable<ProductionLine> Extruders { get; set; }
        #endregion
        #region Fields
        private NozzleChange _selectedExtruderNozzle;

        private RelayCommand _selectFirstDataEntryIfExistsCommand;
        private RelayCommand _addEntityCommand;
        private RelayCommand _removeEntityCommand;
        private RelayCommand _cloneEntityCommand;
        #endregion

        public ExtruderNozzlesViewModel(IDatabaseEntityWindowCloseService windowCloseService) : base(windowCloseService)
        {
            ExtruderNozzles = new ObservableCollection<NozzleChange>(Database.instance.NozzleChangeRepository.GetAll());
            Extruders = Database.instance.ProductionLineRepository.GetAll();
        }

        #region Commands
        public RelayCommand SelectFirstDataEntryIfExistsCommand
        {
            get
            {
                return _selectFirstDataEntryIfExistsCommand ??= new RelayCommand(obj =>
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
        public RelayCommand AddEntityCommand
        {
            get
            {
                return _addEntityCommand ??= new RelayCommand(obj =>
                {
                    NozzleChange nozzle = new NozzleChange();
                    ExtruderNozzles.Add(nozzle);
                    Database.instance.NozzleChangeRepository.Add(nozzle);
                    SelectedExtruderNozzle = nozzle;
                });
            }
        }
        public RelayCommand RemoveEntityCommand
        {
            get
            {
                return _removeEntityCommand ??= new RelayCommand(obj =>
                {
                    Database.instance.NozzleChangeRepository.Delete(SelectedExtruderNozzle);
                    ExtruderNozzles.Remove(SelectedExtruderNozzle);
                    SelectFirstDataEntryIfExistsCommand.Execute(null);
                }, (obj) => SelectedExtruderNozzle != null);
            }
        }
        public RelayCommand CloneEntityCommand
        {
            get
            {
                return _cloneEntityCommand ??= new RelayCommand(obj =>
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

        public override string GetCustomErrorString()
        {
            string result = string.Empty;
            StringBuilder sb = new StringBuilder(result);
            for (int i = 0; i < ExtruderNozzles.Count; i++)
            {
                if (ExtruderNozzles[i].ParentProductionLine == null)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.ExtruderNozzles.Errors.TargetExtruderIsNull"), i));
                }
                if (Math.Sign(ExtruderNozzles[i].ReconfigurationTime) < 0)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.ExtruderNozzles.Errors.ReconfigurationTimeIsNegative"), i));
                }
                if (Math.Sign(ExtruderNozzles[i].NozzleToChange) < 0)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.ExtruderNozzles.Errors.NozzleToChangeIsNegative"), i));
                }
            }
            return sb.ToString();
        }
    }
}
