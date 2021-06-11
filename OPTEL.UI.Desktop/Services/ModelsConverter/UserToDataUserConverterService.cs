using OPTEL.Data.Users;
using OPTEL.UI.Desktop.Services.ModelsConverter.Base;

namespace OPTEL.UI.Desktop.Services.ModelsConverter
{
    public class UserToDataUserConverterService : IModelConverterService<Models.User, OPTEL.Data.Users.User>
    {
        public int ID { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public Models.User Convert(OPTEL.Data.Users.User source)
        {
            return new Models.User(source);
        }

        public OPTEL.Data.Users.User ConvertBack(Models.User source)
        {
            OPTEL.Data.Users.User result;
            switch (source.Type)
            {
                case Models.User.Types.Adminstrator:
                    result = new Administrator();
                    break;
                case Models.User.Types.KnowledgeEngineer:
                    result = new KnowledgeEngineer();
                    break;
                case Models.User.Types.ProductionDirector:
                    result = new ProductionDirector();
                    break;
                default:
                    result = new OPTEL.Data.Users.User();
                    break;
            }
            result.Login = source.DataUser.Login;
            result.Password = source.DataUser.Password;
            return result;
        }
    }
}
