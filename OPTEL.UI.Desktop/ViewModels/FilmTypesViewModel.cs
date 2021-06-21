using EasyLocalization.Localization;
using OPTEL.Data;
using OPTEL.UI.Desktop.Helpers;
using OPTEL.UI.Desktop.Models;
using OPTEL.UI.Desktop.Services.ErrorsListWindows.Base;
using OPTEL.UI.Desktop.Services.WindowClosers.Base;
using OPTEL.UI.Desktop.ViewModels.Core;
using System.Collections.ObjectModel;
using System.Linq;

namespace OPTEL.UI.Desktop.ViewModels
{
    public class FilmTypesViewModel : DatabaseEntityViewModel
    {
        #region Properties
        public FilmType SelectedFilmType
        {
            get => _SelectedFilmType;
            set
            {
                _SelectedFilmType = value;
                IgnoreMarkDataChangedRequestsCommand.Execute(null);
                OnPropertyChanged("SelectedFilmType");
                AcceptMarkDataChangedRequestsCommand.Execute(null);
            }
        }
        public ObservableCollection<FilmType> FilmTypes { get; set; }
        #endregion
        #region Fields
        private FilmType _SelectedFilmType;
        #endregion

        public FilmTypesViewModel(IWindowCloseService windowCloseService, IErrorsListWindowService errorsListService) : base(windowCloseService, errorsListService)
        {
            FilmTypes = new ObservableCollection<FilmType>(Database.instance.FilmTypeRepository.GetAll());
        }

        public override ObservableCollection<Error> GetCustomErrors()
        {
            ObservableCollection<Error> errors = new ObservableCollection<Error>();
            int entryIndex = 0;
            for (int i = 0; i < FilmTypes.Count; i++)
            {
                entryIndex = i + 1;
                if (FilmTypes[i].Article == null || FilmTypes[i].Article.Length == 0)
                {
                    errors.Add(new Error
                    {
                        Content = string.Format(LocalizationManager.Instance.GetValue("Window.FilmTypes.Errors.ArticleIsNull"), entryIndex)
                    });
                }
            }
            return errors;
        }

        public override void SelectFirstDataEntryIfExist()
        {
            FilmType filmType;
            try
            {
                filmType = FilmTypes.First();
            }
            catch
            {
                return;
            }
            SelectedFilmType = filmType;
        }

        public override void AddEntity()
        {
            FilmType type = new FilmType();
            FilmTypes.Add(type);
            Database.instance.FilmTypeRepository.Add(type);
            SelectedFilmType = type;
        }

        public override void RemoveEntity()
        {
            Database.instance.FilmTypeRepository.Delete(SelectedFilmType);
            FilmTypes.Remove(SelectedFilmType);
            SelectFirstDataEntryIfExistsCommand.Execute(null);
        }

        public override bool RemoveEntityExecuteCondition()
        {
            return SelectedFilmType != null;
        }

        public override void CloneEntity()
        {
            FilmType type = new FilmType();
            type.Article = SelectedFilmType.Article;
            FilmTypes.Add(type);
            Database.instance.FilmTypeRepository.Add(type);
            SelectedFilmType = type;
        }

        public override bool CloneEntityExecuteCondition()
        {
            return SelectedFilmType != null;
        }
    }
}
