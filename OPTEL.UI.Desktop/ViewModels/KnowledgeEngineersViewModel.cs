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
                IgnoreMarkDataChangedRequestsCommand.Execute(null);
                OnPropertyChanged("SelectedKnowledgeEngineer");
                AcceptMarkDataChangedRequestsCommand.Execute(null);
            }
        }
        public ObservableCollection<KnowledgeEngineer> KnowledgeEngineers { get; set; }
        #endregion
        #region Fields
        private KnowledgeEngineer _SelectedKnowledgeEngineer;
        #endregion

        public KnowledgeEngineersViewModel(IWindowCloseService windowCloseService, IErrorsListWindowService errorsListService) : base(windowCloseService, errorsListService)
        {
            KnowledgeEngineers = new ObservableCollection<KnowledgeEngineer>(Database.instance.KnowledgeEngineerRepository.GetAll());
        }

        public override void SelectFirstDataEntryIfExist()
        {
            KnowledgeEngineer knowledgeEngineer;
            try
            {
                knowledgeEngineer = KnowledgeEngineers.First();
            }
            catch
            {
                return;
            }
            SelectedKnowledgeEngineer = knowledgeEngineer;
        }

        public override void AddEntity()
        {
            KnowledgeEngineer engineer = new KnowledgeEngineer();
            KnowledgeEngineers.Add(engineer);
            Database.instance.KnowledgeEngineerRepository.Add(engineer);
            SelectedKnowledgeEngineer = engineer;
        }

        public override void RemoveEntity()
        {
            Database.instance.KnowledgeEngineerRepository.Delete(SelectedKnowledgeEngineer);
            KnowledgeEngineers.Remove(SelectedKnowledgeEngineer);
            SelectFirstDataEntryIfExistsCommand.Execute(null);
        }

        public override bool RemoveEntityExecuteCondition()
        {
            return SelectedKnowledgeEngineer != null;
        }

        public override void CloneEntity()
        {
            KnowledgeEngineer engineer = new KnowledgeEngineer();
            engineer.Login = SelectedKnowledgeEngineer.Login;
            engineer.Password = SelectedKnowledgeEngineer.Password;
            KnowledgeEngineers.Add(engineer);
            Database.instance.KnowledgeEngineerRepository.Add(engineer);
            SelectedKnowledgeEngineer = engineer;
        }

        public override bool CloneEntityExecuteCondition()
        {
            return SelectedKnowledgeEngineer != null;
        }
    }
}
