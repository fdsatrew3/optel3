using OPTEL.UI.Desktop.Models.Base;

namespace OPTEL.UI.Desktop.Models
{
    public class User : IDataModel
    {
        public Data.Users.User DataUser { get; set; }

        public bool IsPasswordEncrypted { get; set; }

        public User(Data.Users.User user)
        {
            DataUser = user;
        }
    }
}
