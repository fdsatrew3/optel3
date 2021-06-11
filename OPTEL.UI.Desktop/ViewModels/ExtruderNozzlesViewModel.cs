﻿using EasyLocalization.Localization;
using OPTEL.Data;
using OPTEL.UI.Desktop.Helpers;
using OPTEL.UI.Desktop.Models;
using OPTEL.UI.Desktop.Services.ErrorsListWindows.Base;
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

        public ExtruderNozzlesViewModel(IWindowCloseService windowCloseService, IErrorsListWindowService errorsListService) : base(windowCloseService, errorsListService)
        {
            ExtruderNozzles = new ObservableCollection<NozzleChange>(Database.instance.NozzleChangeRepository.GetAll());
            Extruders = Database.instance.ProductionLineRepository.GetAll();
        }

        #region Commands
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
    }
}
