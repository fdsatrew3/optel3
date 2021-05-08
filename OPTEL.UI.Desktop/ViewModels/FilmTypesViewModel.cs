using EasyLocalization.Localization;
using OPTEL.Data;
using OPTEL.UI.Desktop.Helpers;
using OPTEL.UI.Desktop.ViewModels.Core;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

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
                OnPropertyChanged("SelectedFilmType");
            }
        }
        public ObservableCollection<FilmType> FilmTypes { get; set; }
        #endregion
        #region Fields
        private FilmType _SelectedFilmType;

        private RelayCommand _selectFirstDataEntryIfExistsCommand;
        private RelayCommand _addEntityCommand;
        private RelayCommand _removeEntityCommand;
        private RelayCommand _cloneEntityCommand;
        #endregion

        public FilmTypesViewModel()
        {
            FilmTypes = new ObservableCollection<FilmType>(Database.instance.FilmTypeRepository.GetAll());
        }

        #region Commands
        public RelayCommand SelectFirstDataEntryIfExistsCommand
        {
            get
            {
                return _selectFirstDataEntryIfExistsCommand ??= new RelayCommand(obj =>
                {
                    FilmType coolingLip;
                    try
                    {
                        coolingLip = FilmTypes.First();
                    }
                    catch
                    {
                        return;
                    }
                    SelectedFilmType = coolingLip;
                });
            }
        }
        public RelayCommand AddEntityCommand
        {
            get
            {
                return _addEntityCommand ??= new RelayCommand(obj =>
                {
                    FilmType change = new FilmType();
                    FilmTypes.Add(change);
                    Database.instance.FilmTypeRepository.Add(change);
                    SelectedFilmType = change;
                });
            }
        }
        public RelayCommand RemoveEntityCommand
        {
            get
            {
                return _removeEntityCommand ??= new RelayCommand(obj =>
                {
                    Database.instance.FilmTypeRepository.Delete(SelectedFilmType);
                    FilmTypes.Remove(SelectedFilmType);
                    SelectFirstDataEntryIfExistsCommand.Execute(null);
                }, (obj) => SelectedFilmType != null);
            }
        }
        public RelayCommand CloneEntityCommand
        {
            get
            {
                return _cloneEntityCommand ??= new RelayCommand(obj =>
                {
                    FilmType change = new FilmType();
                    change.Article = SelectedFilmType.Article;
                    FilmTypes.Add(change);
                    Database.instance.FilmTypeRepository.Add(change);
                    SelectedFilmType = change;
                }, (obj) => SelectedFilmType != null);
            }
        }
        #endregion
        public override string GetCustomErrors()
        {
            string result = string.Empty;
            StringBuilder sb = new StringBuilder(result);
            for (int i = 0; i < FilmTypes.Count; i++)
            {
                if (FilmTypes[i].Article == null || FilmTypes[i].Article.Length == 0)
                {
                    sb.AppendLine(string.Format(LocalizationManager.Instance.GetValue("Window.FilmTypes.Errors.ArticleIsNull"), i));
                }
            }
            return sb.ToString();
        }
    }
}
