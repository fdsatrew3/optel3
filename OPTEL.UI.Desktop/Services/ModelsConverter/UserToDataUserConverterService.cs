using OPTEL.Data.Users;
using OPTEL.UI.Desktop.Services.ModelsConverter.Base;

namespace OPTEL.UI.Desktop.Services.ModelsConverter
{
    public class UserToDataUserConverterService<T> : IModelConverterService<Models.User, T> where T : User
    {
        public int ID { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public Models.User Convert(T source)
        {
            return new Models.User(source);
        }

        public T ConvertBack(Models.User source)
        {
            return (T)source.DataUser;
        }
    }
}
