using OPTEL.UI.Desktop.Models.Base;

namespace OPTEL.UI.Desktop.Models
{
    public class ObjectiveFunction : IDataModel
    {
        public enum Types
        {
            Time = 0,
            Cost = 1
        }
        public string Name { get; set; }
        public Types Type { get; set; }
    }
}
