using EasyLocalization.Localization;
using OPTEL.Data;
using OPTEL.UI.Desktop.Models;
using OPTEL.UI.Desktop.Services.ErrorsListWindows.Base;
using OPTEL.UI.Desktop.Services.WindowClosers.Base;
using OPTEL.UI.Desktop.ViewModels.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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
        #endregion

        public ExtruderNozzlesViewModel(IWindowCloseService windowCloseService, IErrorsListWindowService errorsListService) : base(windowCloseService, errorsListService)
        {
            ExtruderNozzles = new ObservableCollection<NozzleChange>(Database.instance.NozzleChangeRepository.GetAll());
            Extruders = Database.instance.ProductionLineRepository.GetAll();
        }


        public override ObservableCollection<Error> GetCustomErrors()
        {
            ObservableCollection<Error> errors = new ObservableCollection<Error>();
            int entryIndex = 0;
            for (int i = 0; i < ExtruderNozzles.Count; i++)
            {
                entryIndex = i + 1;
                if (ExtruderNozzles[i].ParentProductionLine == null)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.ExtruderNozzles.Errors.TargetExtruderIsNull"), entryIndex)
                    });
                }
                if (Math.Sign(ExtruderNozzles[i].ReconfigurationTime) < 0)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.ExtruderNozzles.Errors.ReconfigurationTimeIsNegative"), entryIndex)
                    });
                }
                if (Math.Sign(ExtruderNozzles[i].NozzleToChange) < 0)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.ExtruderNozzles.Errors.NozzleToChangeIsNegative"), entryIndex)
                    });
                }
            }
            return errors;
        }

        public override void SelectFirstDataEntryIfExist()
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
        }

        public override void AddEntity()
        {
            NozzleChange nozzle = new NozzleChange();
            ExtruderNozzles.Add(nozzle);
            Database.instance.NozzleChangeRepository.Add(nozzle);
            SelectedExtruderNozzle = nozzle;
        }

        public override void RemoveEntity()
        {
            Database.instance.NozzleChangeRepository.Delete(SelectedExtruderNozzle);
            ExtruderNozzles.Remove(SelectedExtruderNozzle);
            SelectFirstDataEntryIfExistsCommand.Execute(null);
        }

        public override bool RemoveEntityExecuteCondition()
        {
            return SelectedExtruderNozzle != null;
        }

        public override void CloneEntity()
        {
            NozzleChange nozzle = new NozzleChange();
            nozzle.NozzleToChange = SelectedExtruderNozzle.NozzleToChange;
            nozzle.ParentProductionLine = SelectedExtruderNozzle.ParentProductionLine;
            nozzle.ParentProductionLineID = SelectedExtruderNozzle.ParentProductionLineID;
            nozzle.ReconfigurationTime = SelectedExtruderNozzle.ReconfigurationTime;
            ExtruderNozzles.Add(nozzle);
            Database.instance.NozzleChangeRepository.Add(nozzle);
            SelectedExtruderNozzle = nozzle;
        }

        public override bool CloneEntityExecuteCondition()
        {
            return SelectedExtruderNozzle != null;
        }
    }
}
