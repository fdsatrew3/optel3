using System.ComponentModel.DataAnnotations;

namespace OPTEL.Data
{
    public class CoolingLipChange : Core.IDataObject
    {
        public int ID { get; set; }

        public decimal CoolingLipToChange { get; set; }

        public decimal ReconfigurationTime { get; set; }

        [Required]
        public virtual ProductionLine ParentProductionLine { get; set; }
    }
}
