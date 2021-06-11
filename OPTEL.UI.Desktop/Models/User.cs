using OPTEL.UI.Desktop.Models.Base;

namespace OPTEL.UI.Desktop.Models
{
    public class User : IDataModel
    {
        public enum Types
        {
            Adminstrator,
            KnowledgeEngineer,
            ProductionDirector
        }
        public Types Type { get; set; }

        public Data.Users.User DataUser { get; set; }

        public User(Data.Users.User user)
        {
            DataUser = user;
        }
    }
}
