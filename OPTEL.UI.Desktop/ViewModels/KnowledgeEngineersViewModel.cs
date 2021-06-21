using OPTEL.Data.Users;
using OPTEL.UI.Desktop.Helpers;
using OPTEL.UI.Desktop.Services.ErrorsListWindows.Base;
using OPTEL.UI.Desktop.Services.ModelsConverter;
using OPTEL.UI.Desktop.Services.WindowClosers.Base;
using OPTEL.UI.Desktop.ViewModels.Core;
using OPTEL.UI.Desktop.Views;
using System.Collections.ObjectModel;
using System.Linq;

namespace OPTEL.UI.Desktop.ViewModels
{
    public class KnowledgeEngineersViewModel : DatabaseEntityViewModel
    {
        #region Properties
        public Models.User SelectedKnowledgeEngineer
        {
            get => _selectedKnowledgeEngineer;
            set
            {
                _selectedKnowledgeEngineer = value;
                IgnoreMarkDataChangedRequestsCommand.Execute(null);
                OnPropertyChanged("SelectedKnowledgeEngineer");
                AcceptMarkDataChangedRequestsCommand.Execute(null);
            }
        }
        public ObservableCollection<Models.User> KnowledgeEngineers { get; set; }
        #endregion
        #region Fields
        private Models.User _selectedKnowledgeEngineer;

        private UserToDataUserConverterService<KnowledgeEngineer> _userConverterService;
        #endregion
        public KnowledgeEngineersViewModel(IWindowCloseService windowCloseService, IErrorsListWindowService errorsListService, UserToDataUserConverterService<KnowledgeEngineer> userConverterService) : base(windowCloseService, errorsListService)
        {
            _userConverterService = userConverterService;
            KnowledgeEngineers = new ObservableCollection<Models.User>();
            var knowledgeEngineers = new ObservableCollection<KnowledgeEngineer>(Database.instance.KnowledgeEngineerRepository.GetAll());
            foreach (var knowledgeEngineer in knowledgeEngineers)
            {
                var convertedKnowledgeEngineer = _userConverterService.Convert(knowledgeEngineer);
                convertedKnowledgeEngineer.IsPasswordEncrypted = true;
                KnowledgeEngineers.Add(convertedKnowledgeEngineer);
            }
        }

        public override void SelectFirstDataEntryIfExist()
        {
            Models.User knowledgeEngineer;
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
            KnowledgeEngineer knowledgeEngineer = new KnowledgeEngineer();
            Models.User convertedKnowledgeEngineer = new Models.User(knowledgeEngineer);
            Database.instance.KnowledgeEngineerRepository.Add(knowledgeEngineer);
            KnowledgeEngineers.Add(convertedKnowledgeEngineer);
            SelectedKnowledgeEngineer = convertedKnowledgeEngineer;
        }

        public override void RemoveEntity()
        {
            Database.instance.ProductionDirectorRepository.Delete((ProductionDirector)SelectedKnowledgeEngineer.DataUser);
            KnowledgeEngineers.Remove(SelectedKnowledgeEngineer);
            SelectFirstDataEntryIfExistsCommand.Execute(null);
        }

        public override bool RemoveEntityExecuteCondition()
        {
            return SelectedKnowledgeEngineer != null;
        }

        public override void CloneEntity()
        {
            KnowledgeEngineer knowledgeEngineer = new KnowledgeEngineer();
            knowledgeEngineer.Login = SelectedKnowledgeEngineer.DataUser.Login;
            knowledgeEngineer.Password = SelectedKnowledgeEngineer.DataUser.Password;
            var convertedKnowledgeEngineer = new Models.User(knowledgeEngineer);
            convertedKnowledgeEngineer.IsPasswordEncrypted = SelectedKnowledgeEngineer.IsPasswordEncrypted;
            KnowledgeEngineers.Add(convertedKnowledgeEngineer);
            Database.instance.KnowledgeEngineerRepository.Add(knowledgeEngineer);
            SelectedKnowledgeEngineer = convertedKnowledgeEngineer;
        }

        public override bool CloneEntityExecuteCondition()
        {
            return SelectedKnowledgeEngineer != null;
        }

        public override void OnSaveChanges()
        {
            foreach (var knowledgeEngineer in KnowledgeEngineers)
            {
                if (knowledgeEngineer.IsPasswordEncrypted == false)
                {
                    knowledgeEngineer.DataUser.Password = LoginWindow.Encrypt(knowledgeEngineer.DataUser.Password);
                    knowledgeEngineer.IsPasswordEncrypted = true;
                }
            }
            OnPropertyChanged("SelectedKnowledgeEngineer");
        }
    }
}
