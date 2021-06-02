using System.ComponentModel.DataAnnotations;

namespace OPTEL.Data
{
    public class NozzleChange : Core.IDataObject
    {
        public int ID { get; set; }

        public double NozzleToChange { get; set; }

        public double ReconfigurationTime { get; set; }

        public double Consumption { get; set; }

        public int ParentProductionLineID { get; set; }

        [Required]
        public virtual ProductionLine ParentProductionLine { get; set; }
    }
}
