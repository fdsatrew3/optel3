using OPTEL.Data.Users;
using OPTEL.UI.Desktop.Helpers;
using OPTEL.UI.Desktop.Services.ErrorsListWindows.Base;
using OPTEL.UI.Desktop.Services.WindowClosers.Base;
using OPTEL.UI.Desktop.ViewModels.Core;
using System.Collections.ObjectModel;
using System.Linq;

namespace OPTEL.UI.Desktop.ViewModels
{
    public class KnowledgeEngineersViewModel : DatabaseEntityViewModel
    {
        #region Properties
        public KnowledgeEngineer SelectedKnowledgeEngineer
        {
            get => _SelectedKnowledgeEngineer;
            set
            {
                _SelectedKnowledgeEngineer = value;
                OnPropertyChanged("SelectedKnowledgeEngineer");
                IsDataChanged = false;
            }
        }
        public ObservableCollection<KnowledgeEngineer> KnowledgeEngineers { get; set; }
        #endregion
        #region Fields
        private KnowledgeEngineer _SelectedKnowledgeEngineer;

        private RelayCommand _selectFirstDataEntryIfExistsCommand;
        private RelayCommand _addEntityCommand;
        private RelayCommand _removeEntityCommand;
        private RelayCommand _cloneEntityCommand;
        #endregion

        public KnowledgeEngineersViewModel(IWindowCloseService windowCloseService, IErrorsListWindowService errorsListService) : base(windowCloseService, errorsListService)
        {
            KnowledgeEngineers = new ObservableCollection<KnowledgeEngineer>(Database.instance.KnowledgeEngineerRepository.GetAll());
        }

        #region Commands
        public RelayCommand SelectFirstDataEntryIfExistsCommand
        {
            get
            {
                return _selectFirstDataEntryIfExistsCommand ??= new RelayCommand(obj =>
                {
                    KnowledgeEngineer coolingLip;
                    try
                    {
                        coolingLip = KnowledgeEngineers.First();
                    }
                    catch
                    {
                        return;
                    }
                    SelectedKnowledgeEngineer = coolingLip;
                });
            }
        }
        public RelayCommand AddEntityCommand
        {
            get
            {
                return _addEntityCommand ??= new RelayCommand(obj =>
                {
                    KnowledgeEngineer change = new KnowledgeEngineer();
                    KnowledgeEngineers.Add(change);
                    Database.instance.KnowledgeEngineerRepository.Add(change);
                    SelectedKnowledgeEngineer = change;
                });
            }
        }
        public RelayCommand RemoveEntityCommand
        {
            get
            {
                return _removeEntityCommand ??= new RelayCommand(obj =>
                {
                    Database.instance.KnowledgeEngineerRepository.Delete(SelectedKnowledgeEngineer);
                    KnowledgeEngineers.Remove(SelectedKnowledgeEngineer);
                    SelectFirstDataEntryIfExistsCommand.Execute(null);
                }, (obj) => SelectedKnowledgeEngineer != null);
            }
        }
        public RelayCommand CloneEntityCommand
        {
            get
            {
                return _cloneEntityCommand ??= new RelayCommand(obj =>
                {
                    KnowledgeEngineer change = new KnowledgeEngineer();
                    change.Login = SelectedKnowledgeEngineer.Login;
                    change.Password = SelectedKnowledgeEngineer.Password;
                    KnowledgeEngineers.Add(change);
                    Database.instance.KnowledgeEngineerRepository.Add(change);
                    SelectedKnowledgeEngineer = change;
                }, (obj) => SelectedKnowledgeEngineer != null);
            }
        }
        #endregion
    }
}
