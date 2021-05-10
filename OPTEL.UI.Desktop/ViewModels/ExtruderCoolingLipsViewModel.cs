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
    public class ExtruderCoolingLipsViewModel : DatabaseEntityViewModel
    {
        #region Properties
        public CoolingLipChange SelectedExtruderCoolingLip
        {
            get => _selectedExtruderCoolingLip;
            set
            {
                _selectedExtruderCoolingLip = value;
                OnPropertyChanged("SelectedExtruderCoolingLip");
            }
        }
        public ObservableCollection<CoolingLipChange> ExtruderCoolingLips { get; set; }
        public IEnumerable<ProductionLine> Extruders { get; set; }
        #endregion
        #region Fields
        private CoolingLipChange _selectedExtruderCoolingLip;

        private RelayCommand _selectFirstDataEntryIfExistsCommand;
        private RelayCommand _addEntityCommand;
        private RelayCommand _removeEntityCommand;
        private RelayCommand _cloneEntityCommand;
        #endregion

        public ExtruderCoolingLipsViewModel(IDatabaseEntityWindowCloseService windowCloseService) : base(windowCloseService)
        {
            ExtruderCoolingLips = new ObservableCollection<CoolingLipChange>(Database.instance.CoolingLipChangeRepository.GetAll());
            Extruders = Database.instance.ProductionLineRepository.GetAll();
        }
        
        #region Commands
        public RelayCommand SelectFirstDataEntryIfExistsCommand
        {
            get
            {
                return _selectFirstDataEntryIfExistsCommand ??= new RelayCommand(obj =>
                {
                    CoolingLipChange coolingLip;
                    try
                    {
                        coolingLip = ExtruderCoolingLips.First();
                    }
                    catch
                    {
                        return;
                    }
                    SelectedExtruderCoolingLip = coolingLip;
                });
            }
        }
        public RelayCommand AddEntityCommand
        {
            get
            {
                return _addEntityCommand ??= new RelayCommand(obj =>
                {
                    CoolingLipChange change = new CoolingLipChange();
                    ExtruderCoolingLips.Add(change);
                    Database.instance.CoolingLipChangeRepository.Add(change);
                    SelectedExtruderCoolingLip = change;
                });
            }
        }
        public RelayCommand RemoveEntityCommand
        {
            get
            {
                return _removeEntityCommand ??= new RelayCommand(obj =>
                {
                    Database.instance.CoolingLipChangeRepository.Delete(SelectedExtruderCoolingLip);
                    ExtruderCoolingLips.Remove(SelectedExtruderCoolingLip);
                    SelectFirstDataEntryIfExistsCommand.Execute(null);
                }, (obj) => SelectedExtruderCoolingLip != null);
            }
        }
        public RelayCommand CloneEntityCommand
        {
            get
            {
                return _cloneEntityCommand ??= new RelayCommand(obj =>
                {
                    CoolingLipChange change = new CoolingLipChange();
                    change.CoolingLipToChange = SelectedExtruderCoolingLip.CoolingLipToChange;
                    change.ParentProductionLine = SelectedExtruderCoolingLip.ParentProductionLine;
                    change.ParentProductionLineID = SelectedExtruderCoolingLip.ParentProductionLineID;
                    change.ReconfigurationTime = SelectedExtruderCoolingLip.ReconfigurationTime;
                    ExtruderCoolingLips.Add(change);
                    Database.instance.CoolingLipChangeRepository.Add(change);
                    SelectedExtruderCoolingLip = change;
                }, (obj) => SelectedExtruderCoolingLip != null);
            }
        }
        #endregion

        public override string GetCustomErrorString()
        {
            string result = string.Empty;
            StringBuilder sb = new StringBuilder(result);
            for (int i = 0; i < ExtruderCoolingLips.Count; i++)
            {
                if (ExtruderCoolingLips[i].ParentProductionLine == null)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.ExtruderCoolingLips.Errors.TargetExtruderIsNull"), i));
                }
                if (Math.Sign(ExtruderCoolingLips[i].ReconfigurationTime) < 0)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.ExtruderCoolingLips.Errors.ReconfigurationTimeIsNegative"), i));
                }
                if (Math.Sign(ExtruderCoolingLips[i].CoolingLipToChange) < 0)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.ExtruderCoolingLips.Errors.CoolingLipToChangeIsNegative"), i));
                }
            }
            return sb.ToString();
        }
    }
}
