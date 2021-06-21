using OPTEL.UI.Desktop.Models.Base;

namespace OPTEL.UI.Desktop.Models
{
    public class PlanningAlgorithm : IDataModel
    {
        public enum Types
        {
            Genetic = 0,
            Bruteforce = 1
        }
        public string Name { get; set; }
        public Types Type { get; set; }
    }
}
