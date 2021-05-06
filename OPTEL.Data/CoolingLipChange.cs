using System.ComponentModel.DataAnnotations;

namespace OPTEL.Data
{
    public class CoolingLipChange : Core.IDataObject
    {
        public int ID { get; set; }

        public double CoolingLipToChange { get; set; }

        public double ReconfigurationTime { get; set; }

        [Required]
        public virtual ProductionLine ParentProductionLine { get; set; }
    }
}
